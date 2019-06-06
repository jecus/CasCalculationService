using System;
using System.Threading.Tasks;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using BusinessLayer.Views;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public class MTOPCalculator : IMTOPCalculator
	{
		public async Task CalculateDirective(MaintenanceDirectiveView directive, AverageUtilization averageUtilization)
		{
			double hours = 0, cycles = 0, days = 0;
			double hoursPhase = -1, cyclesPhase = -1, daysPhase = -1;

			directive.PhaseThresh = new Lifelength(0, 0, 0);

			var thresh = !directive.Threshold.FirstPerformanceSinceNew.IsNullOrZero() ? directive.Threshold.FirstPerformanceSinceNew : directive.Threshold.FirstPerformanceSinceEffectiveDate;

			if (thresh.Days.HasValue)
			{
				hours = (double)(thresh.Days * averageUtilization.Hours);
				cycles = hours / averageUtilization.Cycles;
				days = (double)thresh.Days;
			}
			else if (thresh.Hours.HasValue)
			{
				hours = (double)thresh.Hours;
				cycles = hours / averageUtilization.Cycles;
				days = (double)(thresh.Hours / averageUtilization.HoursPerDay);
			}
			else if (thresh.Cycles.HasValue)
			{
				cycles = (double)thresh.Cycles;
				days = (double)(thresh.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
				hours = days * averageUtilization.Hours;
			}


			if (cycles > thresh.Cycles && hours > thresh.Hours)
			{
				var cycleDays = (int)(cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));

				if (cycleDays > days)
				{
					daysPhase = (int)(thresh.Hours / averageUtilization.HoursPerDay);
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					hoursPhase = daysPhase * averageUtilization.Hours;
				}
				else
				{
					daysPhase = (int)(thresh.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					hoursPhase = daysPhase * averageUtilization.Hours;

					if (hoursPhase > thresh.Hours)
					{
						daysPhase = (int)(thresh.Hours / averageUtilization.HoursPerDay);
						cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
						hoursPhase = daysPhase * averageUtilization.Hours;
					}
				}
			}
			else if (cycles > thresh.Cycles)
			{
				daysPhase = (int)(thresh.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
				cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
				hoursPhase = daysPhase * averageUtilization.Hours;

				if (hoursPhase > thresh.Hours)
				{
					daysPhase = (int)(thresh.Hours / averageUtilization.HoursPerDay);
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					hoursPhase = daysPhase * averageUtilization.Hours;
				}
			}
			else if (hours > thresh.Hours)
			{
				daysPhase = (int)(thresh.Hours / averageUtilization.HoursPerDay);
				cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
				hoursPhase = daysPhase * averageUtilization.Hours;

				if (cyclesPhase > thresh.Cycles)
				{
					daysPhase = (int)(thresh.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
					hoursPhase = daysPhase * averageUtilization.Hours;
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
				}
			}

			directive.PhaseThresh.Hours = (int)Math.Round(hoursPhase > -1 ? hoursPhase : hours);
			directive.PhaseThresh.Cycles = (int)Math.Round(cyclesPhase > -1 ? cyclesPhase : cycles);
			directive.PhaseThresh.Days = (int)Math.Round(daysPhase > -1 ? daysPhase : days);

			var repeat = directive.Threshold.RepeatInterval;
			directive.PhaseRepeat = new Lifelength(0, 0, 0);

			if (!repeat.IsNullOrZero())
			{
				hoursPhase = -1;
				cyclesPhase = -1;
				daysPhase = -1;

				if (repeat.Days.HasValue)
				{
					hours = (double)(repeat.Days * averageUtilization.Hours);
					cycles = hours / averageUtilization.Cycles;
					days = (double)repeat.Days;
				}
				else if (repeat.Hours.HasValue)
				{
					hours = (double)repeat.Hours;
					cycles = hours / averageUtilization.Cycles;
					days = (double)(repeat.Hours / averageUtilization.HoursPerDay);
				}
				else if (repeat.Cycles.HasValue)
				{
					cycles = (double)repeat.Cycles;
					days = (double)(repeat.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
					hours = days * averageUtilization.Hours;
				}


				if (cycles > repeat.Cycles && hours > repeat.Hours)
				{
					var cycleDays = (int)(cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));

					if (cycleDays > days)
					{
						daysPhase = (int)(repeat.Hours / averageUtilization.HoursPerDay);
						cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
						hoursPhase = daysPhase * averageUtilization.Hours;
					}
					else
					{
						daysPhase = (int)(repeat.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
						cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
						hoursPhase = daysPhase * averageUtilization.Hours;

						if (hoursPhase > repeat.Hours)
						{
							daysPhase = (int)(repeat.Hours / averageUtilization.HoursPerDay);
							cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
							hoursPhase = daysPhase * averageUtilization.Hours;
						}
					}
				}
				else if (cycles > repeat.Cycles)
				{
					daysPhase = (int)(repeat.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					hoursPhase = daysPhase * averageUtilization.Hours;

					if (hoursPhase > repeat.Hours)
					{
						daysPhase = (int)(repeat.Hours / averageUtilization.HoursPerDay);
						cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
						hoursPhase = daysPhase * averageUtilization.Hours;
					}
				}
				else if (hours > repeat.Hours)
				{
					daysPhase = (int)(repeat.Hours / averageUtilization.HoursPerDay);
					cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					hoursPhase = daysPhase * averageUtilization.Hours;

					if (cyclesPhase > repeat.Cycles)
					{
						daysPhase = (int)(repeat.Cycles / (averageUtilization.Hours / averageUtilization.CyclesPerDay));
						hoursPhase = daysPhase * averageUtilization.Hours;
						cyclesPhase = daysPhase * (averageUtilization.Hours / averageUtilization.CyclesPerDay);
					}
				}

				directive.PhaseRepeat.Hours = (int)Math.Round(hoursPhase > -1 ? hoursPhase : hours);
				directive.PhaseRepeat.Cycles = (int)Math.Round(cyclesPhase > -1 ? cyclesPhase : cycles);
				directive.PhaseRepeat.Days = (int)Math.Round(daysPhase > -1 ? daysPhase : days);
			}

		}
	}
}