using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.CalcView;
using BusinessLayer.Repositiries;
using BusinessLayer.Vendors;
using BusinessLayer.Views;
using Entity;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public class PerformanceCalculator : IPerformanceCalculator
	{
		private readonly ICalculator _calculator;
		private readonly IMTOPCalculator _mtopCalculator;
		private readonly IAverageUtilizationRepository _averageUtilizationRepository;
		private readonly IComponentRepository _componentRepository;

		public PerformanceCalculator(ICalculator calculator, 
			IMTOPCalculator mtopCalculator,
			IAverageUtilizationRepository averageUtilizationRepository,
			IComponentRepository componentRepository)
		{
			_calculator = calculator;
			_mtopCalculator = mtopCalculator;
			_averageUtilizationRepository = averageUtilizationRepository;
			_componentRepository = componentRepository;
		}

		public async Task<List<NextPerformance>> NextPerformanceForComponent(int componentId)
		{
			var comp = await _componentRepository.GetComponentByIdAsync(componentId);
			await GetNextPerformance(comp);
			return comp.NextPerformances;
		}

		public async Task<Dictionary<int,List<NextPerformance>>> NextPerformanceForComponents(List<int> componentIds)
		{
			var res = new Dictionary<int, List<NextPerformance>>();
			var componentViews = await _componentRepository.GetComponentsAsync(componentIds);

			foreach (var componentView in componentViews)
			{
				await GetNextPerformance(componentView);
				res.Add(componentView.Id, componentView.NextPerformances);
			}

			return res;
		}

		public async Task<List<NextPerformance>> NextPerformanceForComponentDirective(int componentDirectiveId)
		{
			var cd = await _componentRepository.GetComponentDirectiveByIdAsync(componentDirectiveId);
			await GetNextPerformance(cd);
			return cd.NextPerformances;
		}

		public async Task<Dictionary<int, List<NextPerformance>>> NextPerformanceForComponentDirectives(List<int> componentDirectiveIds)
		{
			var res = new Dictionary<int, List<NextPerformance>>();
			var directives = await _componentRepository.GetComponentDirectivessAsync(componentDirectiveIds);

			foreach (var directive in directives)
			{
				await GetNextPerformance(directive);
				res.Add(directive.Id, directive.NextPerformances);
			}

			return res;
		}


		#region private void GetNextPerformance(Document directive)

		/// <summary>
		/// Расчитывает следующее выполнение Document вместе с дополнительными параметрами
		/// </summary>
		/// <param name="directive">Принимает документ</param>
		private void GetNextPerformance(DocumentView directive)
		{
			// Если следующего выполнения нет - то все остальные данные не имеют смысла
			if ((directive.IssueValidTo == false && directive.RevisionValidTo == false) || directive.IsClosed)
			{
				directive.Remains = null;
				directive.RevisionRemains = null;
				directive.NextPerformanceDate = null;
				directive.Condition = ConditionState.NotEstimated;
			}
			else // Директива имеет следующее выполнение 
			{
				Lifelength issueNotify = null, revisionNotify = null, performanceSource = null;

				// расчитываем remains
				if (directive.IssueValidTo)
				{
					var t = directive.IssueDateValidTo - DateTime.Today;
					directive.Remains = new Lifelength(t.Days, null, null);

					issueNotify = directive.IssueNotify > 0
						? new Lifelength(directive.IssueNotify, null, null)
						: null;
					performanceSource = new Lifelength(directive.Remains);//TODO:(Evgenii Babak) выяснить почему у performanceSource такое же значение, что и у Remains
				}

				if (directive.RevisionValidTo)
				{
					var t = directive.RevisionDateValidTo - DateTime.Today;
					directive.RevisionRemains = new Lifelength(t.Days, null, null);

					revisionNotify = directive.RevisionNotify > 0
						? new Lifelength(directive.RevisionNotify, null, null)
						: null;
					performanceSource = new Lifelength(directive.RevisionRemains);//TODO:(Evgenii Babak) выяснить почему у performanceSource такое же значение, что и у RevisionRemains
				}

				if (directive.Remains != null && directive.RevisionRemains != null)
				{
					directive.Condition = computeConditionState(performanceSource, directive.RevisionRemains, null, revisionNotify, x => x.IsOverdue());
					directive.NextPerformanceDate = directive.RevisionDateValidTo;
				}
				else if (directive.Remains != null)
				{
					directive.Condition = computeConditionState(performanceSource, directive.Remains, null, issueNotify, x => x.IsOverdue());
					directive.NextPerformanceDate = directive.IssueDateValidTo;
				}
				else
				{
					directive.Condition = computeConditionState(performanceSource, directive.RevisionRemains, null, revisionNotify, x => x.IsOverdue());
					directive.NextPerformanceDate = directive.RevisionDateValidTo;
				}
			}
		}

		#endregion

		#region private void GetNextPerformance(IDirective directive)

		/// <summary>
		/// Расчитывает следующее выполнение directive вместе с дополнительными параметрами
		/// </summary>
		/// <param name="directive">Принимает директиву</param>
		/// <param name="forecast"></param>
		private async Task GetNextPerformance(IDirective directive)
		{
			if (directive == null)
				return;

			directive.ResetMathData();

			if (directive.IsClosed || directive.Threshold == null)
				return;

			IThreshold threshold;
			//if (directive is MaintenanceDirectiveView && ((MaintenanceDirectiveView)directive).MaintenanceCheck != null)
			//	threshold = ((MaintenanceDirectiveView)directive).MaintenanceCheck.Threshold;
			//else threshold = directive.Threshold;

			threshold = directive.Threshold;

			var last = Lifelength.Null;
			var current = await _calculator.GetFlightLifelengthOnEndOfDay(directive.LifeLengthParent, DateTime.Today);
			NextPerformance np;
			NextPerformance lastNp;

			var au = await _averageUtilizationRepository.GetAverageUtillization(directive);

			for (;;)
			{
				np = null;

				#region Определение возможности след. выполения и ресурса на котором оно произойдет

				if (directive.NextPerformances.Count == 0)
				{
					if (directive.LastPerformance == null) // директива ни разу не выполнялась
					{
						// Расчитываем условие первого выполнения 
						// получаем ресурс агрегата при котором директива должна будет выполнена с момента вступления директивы в действие
						if ((threshold.FirstPerformanceSinceEffectiveDate != null &&
							 !threshold.FirstPerformanceSinceEffectiveDate.IsNullOrZero())
							||
							(threshold.FirstPerformanceSinceNew != null &&
							 !threshold.FirstPerformanceSinceNew.IsNullOrZero()))
						{
							np = new NextPerformance { Parent = directive };

							Lifelength sinceEffDate = Lifelength.Null;
							if (threshold.FirstPerformanceSinceEffectiveDate != null &&
								!threshold.FirstPerformanceSinceEffectiveDate.IsNullOrZero())
							{
								sinceEffDate = await _calculator.GetFlightLifelengthOnStartOfDay(directive.LifeLengthParent, threshold.EffectiveDate);
								sinceEffDate.Resemble(threshold.FirstPerformanceSinceEffectiveDate);
								if (threshold.RepeatInterval.CalendarValue != null)
									sinceEffDate.Add(threshold.EffectiveDate, threshold.FirstPerformanceSinceEffectiveDate);
								else sinceEffDate.Add(threshold.FirstPerformanceSinceEffectiveDate);
							}

							// с момента производства
							Lifelength sinceNew = Lifelength.Null;
							if (threshold.FirstPerformanceSinceNew != null &&
								!threshold.FirstPerformanceSinceNew.IsNullOrZero())
							{
								sinceNew = threshold.FirstPerformanceSinceNew;

								if (directive is MaintenanceDirectiveView)
								{
									var d = directive as MaintenanceDirectiveView;

									await _mtopCalculator.CalculateDirective(d, au);
									sinceNew = d.PhaseThresh;
								}
							}

							if (directive is ComponentView)
							{
								var lastTr = ((ComponentView)directive).TransferRecords.GetLast();
								if (lastTr != null)
									np.PerformanceNum = 1 + (lastTr.PerformanceNum <= 0 ? 1 : lastTr.PerformanceNum);
								else np.PerformanceNum = 1;
							}
							else np.PerformanceNum = directive.NextPerformances.Count + 1;


							np.PerformanceSource = new Lifelength(sinceNew);
							if (threshold.FirstPerformanceConditionType == ThresholdConditionType.WhicheverFirst)
								np.PerformanceSource.SetMin(sinceEffDate);
							else np.PerformanceSource.SetMax(sinceEffDate);
						}
					}
					else // Директива уже выполнялась 
					{
						// Расчитываем условие следующего выполнения
						// Следующее выполнение = ресурс базового агрегата на момент прошлого выполнения директивы + repeat interval
						if (threshold.PerformRepeatedly && threshold.RepeatInterval != null &&
							!threshold.RepeatInterval.IsNullOrZero())
						{
							np = new NextPerformance { Parent = directive };

							last = directive.LastPerformance.OnLifelength;


							if (directive is MaintenanceDirectiveView)
							{
								var d = directive as MaintenanceDirectiveView;

								await _mtopCalculator.CalculateDirective(d, au);
								int lastPerfNum = directive.LastPerformance.PerformanceNum <= 0 ? 1 : directive.LastPerformance.PerformanceNum;
								np.PerformanceNum = lastPerfNum + directive.NextPerformances.Count + 1;
								np.PerformanceSource = new Lifelength(last);
								if (d.PhaseRepeat != null && !d.PhaseRepeat.IsNullOrZero())
									np.PerformanceSource.Add(Convert.ToDateTime(directive.LastPerformance.RecordDate), d.PhaseRepeat);
								else np.PerformanceSource.Add(d.PhaseThresh);
								// Убираем не нужные параметры

								if (d.PhaseRepeat != null && !d.PhaseRepeat.IsNullOrZero())
									np.PerformanceSource.Resemble(d.PhaseRepeat);
								else np.PerformanceSource.Resemble(d.PhaseThresh);
							}
							else
							{
								int lastPerfNum = directive.LastPerformance.PerformanceNum <= 0 ? 1 : directive.LastPerformance.PerformanceNum;
								np.PerformanceNum = lastPerfNum + directive.NextPerformances.Count + 1;
								np.PerformanceSource = new Lifelength(last);
								if (threshold.RepeatInterval.CalendarValue != null)
									np.PerformanceSource.Add(Convert.ToDateTime(directive.LastPerformance.RecordDate), threshold.RepeatInterval);
								else np.PerformanceSource.Add(threshold.RepeatInterval);
								// Убираем не нужные параметры
								np.PerformanceSource.Resemble(threshold.RepeatInterval);
							}
						}
					}
				}
				else
				{
					// Расчитываем условие следующего выполнения
					// Следующее выполнение = ресурс базового агрегата на момент прошлого выполнения директивы + repeat interval
					if (threshold.PerformRepeatedly && threshold.RepeatInterval != null &&
						!threshold.RepeatInterval.IsNullOrZero())
					{
						np = new NextPerformance { Parent = directive };

						lastNp = directive.NextPerformances.Last();
						last = lastNp.PerformanceSource;

						if (directive is MaintenanceDirectiveView)
						{
							var d = directive as MaintenanceDirectiveView;

							await _mtopCalculator.CalculateDirective(d, au);

							np.PerformanceNum = lastNp.PerformanceNum + 1;
							np.PerformanceSource = new Lifelength(last);
							if (d.PhaseRepeat != null && !d.PhaseRepeat.IsNullOrZero())
								np.PerformanceSource.Add(d.PhaseRepeat);
							else np.PerformanceSource.Add(d.PhaseThresh);
							// Убираем не нужные параметры
							if (d.PhaseRepeat != null && !d.PhaseRepeat.IsNullOrZero())
								np.PerformanceSource.Resemble(d.PhaseRepeat);
							else np.PerformanceSource.Resemble(d.PhaseThresh);
						}
						else
						{
							np.PerformanceNum = lastNp.PerformanceNum + 1;
							np.PerformanceSource = new Lifelength(last);
							if (lastNp.PerformanceDate != null && threshold.RepeatInterval.CalendarValue != null)
								np.PerformanceSource.Add(Convert.ToDateTime(lastNp.PerformanceDate), threshold.RepeatInterval);
							else np.PerformanceSource.Add(threshold.RepeatInterval);
							// Убираем не нужные параметры
							np.PerformanceSource.Resemble(threshold.RepeatInterval);
						}
					}
				}

				#endregion

				if (np == null)
				{
					break;
				}

				#region Определение остатка ресурса (Remain), ресурса предупреждения(Notify),
				//условия(ThresholdConditionType) и 
				//приблизительной даты (approximate date) следующего выполнения


				if (directive is ComponentView && ((ComponentView)directive).LLPMark && ((ComponentView)directive).LLPCategories)
				{
					//TODO:хз зачем llp тут
					//np.Remains = ((ComponentView)directive).LLPRemains;
				}
				else
				{
					if (directive.LastPerformance != null)
					{
						np.LimitOverdue = new Lifelength(directive.Threshold.RepeatInterval);
						np.LimitNotify = new Lifelength(directive.Threshold.RepeatInterval);
						np.LimitNotify.Substract(directive.Threshold.FirstNotification);

						np.LimitOverdue.Add(directive.LastPerformance.OnLifelength);
						np.LimitNotify.Add(directive.LastPerformance.OnLifelength);

						np.LimitOverdue.Resemble(directive.Threshold.RepeatInterval);
						np.LimitNotify.Resemble(directive.Threshold.RepeatInterval);
					}
					else
					{
						if (directive.Threshold.FirstPerformanceSinceEffectiveDate != null && !directive.Threshold.FirstPerformanceSinceEffectiveDate.IsNullOrZero())
						{
							np.LimitOverdue = new Lifelength(np.PerformanceSource);
							np.LimitNotify = new Lifelength(np.PerformanceSource);
							np.LimitNotify.Substract(directive.Threshold.FirstNotification);

							np.LimitOverdue.Resemble(np.PerformanceSource);
							np.LimitNotify.Resemble(np.PerformanceSource);
						}
						else
						{
							np.LimitOverdue = new Lifelength(directive.Threshold.FirstPerformanceSinceNew);
							np.LimitNotify = new Lifelength(directive.Threshold.FirstPerformanceSinceNew);
							np.LimitNotify.Substract(directive.Threshold.FirstNotification);

							np.LimitOverdue.Resemble(directive.Threshold.FirstPerformanceSinceNew);
							np.LimitNotify.Resemble(directive.Threshold.FirstPerformanceSinceNew);
						}
					}


					if (directive is ComponentView)
					{
						np.WarrantlyRemains = new Lifelength(((ComponentView)directive).Warranty);
						np.WarrantlyRemains.Substract(current); // remains = next - current

						np.WarrantlyRemains.Resemble(np.LimitOverdue);
					}
					else if (directive is ComponentDirectiveView)
					{
						np.WarrantlyRemains = new Lifelength(((ComponentDirectiveView)directive).Threshold.Warranty);
						np.WarrantlyRemains.Substract(current); // remains = next - current

						np.WarrantlyRemains.Resemble(np.LimitOverdue);
					}

					//np.Remains = new Lifelength(np.LimitOverdue);
					np.Remains = new Lifelength(np.PerformanceSource);
					np.Remains.Substract(current); // remains = next - current


					np.Remains.Resemble(np.LimitOverdue);
				}

				if (np.Remains == null)
					np.Remains = Lifelength.Null;

				// Condition State и Approximate Date расчитывается по разному в зависимости от Whichever First и Whichever Later
				// Whichever First и Whichever Later - разные для первого выполнения и повтороного выполнения
				ThresholdConditionType conditionType;
				Lifelength notify, notifyRemains;
				if (directive.LastPerformance == null)
				{
					conditionType = threshold.FirstPerformanceConditionType;
					notify = threshold.FirstNotification != null
								 ? new Lifelength(threshold.FirstNotification)
								 : null;
				}
				else if (threshold.RepeatInterval != null && !threshold.RepeatInterval.IsNullOrZero())
				{
					conditionType = directive.Threshold.RepeatPerformanceConditionType;
					notify = directive.Threshold.RepeatNotification != null
								 ? new Lifelength(directive.Threshold.RepeatNotification)
								 : null;
				}
				else
				{
					throw new Exception("1234: Incorrect directive status");
				}


				np.PerformanceDate = null;

				if (directive.NextPerformances.Count > 0 &&
				    directive.NextPerformances.Last().PerformanceDate != null)
				{
					//к дате пред. выполнения добавляется количество дней
					//за которое будет израсходован повторяющийся интервал директивы
					//с учетом заданной средней утилизации
					double? days = threshold.RepeatInterval.Days;
					if (days != null)
						np.PerformanceDate = directive.NextPerformances.Last().PerformanceDate.Value.AddDays(Convert.ToDouble(days));
					else np.PerformanceDate = null;
				}
				else if (directive.LastPerformance != null)
				{
					double? days = threshold.RepeatInterval.Days;
					if (days != null)
						np.PerformanceDate = directive.LastPerformance.RecordDate.AddDays(Convert.ToDouble(days));
					else np.PerformanceDate = null;
				}
				else
				{
					double? days = np.PerformanceSource.Days;
					if (days != null)
					{
						np.PerformanceDate = _calculator.GetManufactureDate(directive.LifeLengthParent).AddDays(Convert.ToDouble(days));
					}
					else np.PerformanceDate = null;
				}

				#endregion

				directive.NextPerformances.Add(np);

				#region Расчет текущего состояния задачи в зависимости от условий выполнения

				if (conditionType == ThresholdConditionType.WhicheverFirst)
				{
					// задано только одно условие выполнения - считаем по whichever first
					// whichever first

					// состояние директивы - просрочена или нормально
					np.Condition = computeConditionState(directive, np.LimitNotify, np.LimitOverdue, np.Remains, current, notify, x => x.IsOverdue(), x => x.IsLessByAnyParameter(notify), ThresholdConditionType.WhicheverFirst);

				}
				else // whichever later
				{
					// директива просрочена только в том случае, когда она просрочена по всем параметрам
					np.Condition = computeConditionState(directive, np.LimitNotify, np.LimitOverdue, np.Remains, current, notify, x => x.IsAllOverdue(), x => x.IsLess(notify), ThresholdConditionType.WhicheverLater);
				}

				#endregion
			}
		}

		#endregion


		// Вспомогательные методы
		private ConditionState computeConditionState(Lifelength performanceSource, Lifelength remains, Lifelength current, Lifelength notify, Func<Lifelength, bool> getIsOverdueFunc)
		{
			if (notify != null && !notify.IsNullOrZero())
			{
				if (getIsOverdueFunc(remains))
					return ConditionState.Overdue;

				var notifyRemains = new Lifelength(performanceSource);
				notifyRemains.Substract(notify);

				if (current != null && !current.IsNullOrZero())
					notifyRemains.Substract(current);

				notifyRemains.Resemble(notify);
				return getIsOverdueFunc(notifyRemains) ? ConditionState.Notify : ConditionState.Satisfactory;
			}

			return getIsOverdueFunc(remains) ? ConditionState.Overdue : ConditionState.Satisfactory;
		}

		private ConditionState computeConditionState(IDirective directive, Lifelength limitNotify,
			Lifelength limitOverdue, Lifelength remains,
			Lifelength current, Lifelength notify, Func<Lifelength, bool> getIsOverdueFunc,
			Func<Lifelength, bool> getIsLessFunc, ThresholdConditionType whicheverFirst)
		{
			if (notify != null && !notify.IsNullOrZero())
			{
				if (directive is ComponentView && ((ComponentView)directive).LLPMark && ((ComponentView)directive).LLPCategories)
				{
					return getIsOverdueFunc(remains)
						? ConditionState.Overdue
						: (getIsLessFunc(notify)
							? ConditionState.Notify
							: ConditionState.Satisfactory);
				}


				if (current.IsGreaterByAnyParameter(limitOverdue))
					return ConditionState.Overdue;

				if (limitOverdue.IsGreaterByAnyParameter(current) && current.IsGreaterByAnyParameter(limitNotify))
					return ConditionState.Notify;

				return ConditionState.Satisfactory;

			}

			return getIsOverdueFunc(remains) ? ConditionState.Overdue : ConditionState.Satisfactory;
		}
	}
}