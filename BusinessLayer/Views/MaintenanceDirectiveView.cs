using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class MaintenanceDirectiveView : BaseView, IDirective
	{
		#region IDirective
		public BaseView LifeLengthParent => null;

		private List<NextPerformance> _nextPerformances;
		public List<NextPerformance> NextPerformances => _nextPerformances ?? (_nextPerformances = new List<NextPerformance>());
		public IThreshold Threshold { get; set; }
		public DirectiveRecordView LastPerformance => PerformanceRecords.GetLast();
		public bool IsClosed { get; set; }
		public void ResetMathData()
		{
			NextPerformances.Clear();
		}

		#endregion

		public List<DirectiveRecordView> PerformanceRecords { get; set; }

		public Lifelength PhaseThresh { get; set; }
		public Lifelength PhaseRepeat { get; set; }


		public MaintenanceDirectiveView(MaintenanceDirective source)
		{
			if (source == null)
				return;
			Threshold = CalcView.Threshold.ConvertForCMaintenanceDirective(source.Threshold);
			IsClosed = source.IsClosed;
			PerformanceRecords = new List<DirectiveRecordView>(source.PerformanceRecords?.Select(i => new DirectiveRecordView(i)));
		}
	}
}