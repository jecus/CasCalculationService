using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentDirectiveView : BaseView, IDirective
	{
		public int DirectiveType { get; set; }

		#region MyRegionImplementation of IDirective

		public BaseView LifeLengthParent =>  ParentComponent;

		private List<NextPerformance> _nextPerformances;
		public List<NextPerformance> NextPerformances => _nextPerformances ?? (_nextPerformances = new List<NextPerformance>());

		public IThreshold Threshold { get; set; }
		public DirectiveRecordView LastPerformance => PerformanceRecords.GetLast();
		public bool IsClosed { get; set; }
		public void ResetMathData()
		{
			NextPerformances.Clear();;
		}

		#endregion

		public ComponentView ParentComponent { get; set; }

		public int? ComponentId { get; set; }

		public List<DirectiveRecordView> PerformanceRecords { get; set; }


		public ComponentDirectiveView(ComponentDirective source)
		{
			if (source == null)
				return;
			Id = source.Id;
			DirectiveType = source.DirectiveType;
			ComponentId = source.ComponentId;
			IsClosed = source.IsClosed;
			Threshold = CalcView.Threshold.ConvertForComponentDirective(source.Threshold);
			PerformanceRecords = new List<DirectiveRecordView>(source.PerformanceRecords?.Select(i => new DirectiveRecordView(i)));
			ParentComponent =new ComponentView(source.Component); 
		}
	}
}