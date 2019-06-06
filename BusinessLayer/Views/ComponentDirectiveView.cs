﻿using System.Collections.Generic;
using System.Linq;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentDirectiveView : BaseView, IDirective
	{
		public int DirectiveType { get; set; }

		public IThreshold Threshold { get; set; }

		public int? ComponentId { get; set; }

		public List<DirectiveRecordView> PerformanceRecords { get; set; }


		public ComponentDirectiveView(ComponentDirective source)
		{
			Id = source.Id;
			DirectiveType = source.DirectiveType;
			ComponentId = source.ComponentId;
			Threshold = CalcView.Threshold.ConvertForComponentDirective(source.Threshold);
			PerformanceRecords = new List<DirectiveRecordView>(source.PerformanceRecords.Select(i => new DirectiveRecordView(i)));
		}
	}
}