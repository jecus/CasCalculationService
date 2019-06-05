using System;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.Vendors;
using BusinessLayer.Views;
using Entity;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public class PerformanceCalculator : IPerformanceCalculator
	{
		public PerformanceCalculator()
		{
			
		}



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