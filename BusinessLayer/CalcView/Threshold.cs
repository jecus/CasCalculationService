using System;
using BusinessLayer.Vendors;
using Entity;

namespace BusinessLayer.CalcView
{
	public class Threshold : IThreshold
	{
		#region Implementation of IThreshold

		public ThresholdConditionType FirstPerformanceConditionType { get; set; }
		public ThresholdConditionType RepeatPerformanceConditionType { get; set; }
		public bool PerformRepeatedly { get; set; }
		public DateTime EffectiveDate { get; set; }
		public Lifelength FirstPerformanceSinceNew { get; set; }
		public Lifelength FirstPerformanceSinceEffectiveDate { get; set; }
		public Lifelength FirstNotification { get; set; }
		public Lifelength RepeatInterval { get; set; }
		public Lifelength RepeatNotification { get; set; }
		#endregion
	}
}