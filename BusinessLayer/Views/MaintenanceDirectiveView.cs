using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class MaintenanceDirectiveView : BaseView, IDirective
	{
		public IThreshold Threshold { get; set; }

		public List<DirectiveRecordView> PerformanceRecords { get; set; }


		public Lifelength PhaseThresh { get; set; }
		public Lifelength PhaseRepeat { get; set; }


		public MaintenanceDirectiveView(MaintenanceDirective source)
		{
			Threshold = CalcView.Threshold.ConvertForCMaintenanceDirective(source.Threshold);
			PerformanceRecords = new List<DirectiveRecordView>(source.PerformanceRecords.Select(i => new DirectiveRecordView(i)));
		}
	}
}