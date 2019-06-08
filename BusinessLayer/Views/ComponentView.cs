using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator;
using BusinessLayer.Calculator.Views;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentView : BaseView, IDirective
	{
		public int AircaraftId { get; set; }

		public string PartNumber { get; set; }

		public DateTime ManufactureDate { get; set; }

		public bool IsBaseComponent { get; set; }

		public bool LLPMark { get; set; }

		public bool LLPCategories { get; set; }

		public Lifelength Warranty
		{
			get { return _threshold.Warranty; }
			set { _threshold.Warranty = value; }
		}

		public Lifelength WarrantyNotify
		{
			get { return _threshold.WarrantyNotification; }
			set { _threshold.WarrantyNotification = value; }
		}

		public Lifelength LifeLimit
		{
			get { return _threshold.FirstPerformanceSinceNew; }
			set { _threshold.FirstPerformanceSinceNew = value; }
		}

		public Lifelength LifeLimitNotify
		{
			get { return _threshold.FirstNotification; }
			set { _threshold.FirstNotification = value; }
		}



		public AverageUtilization AverageUtilization { get; set; }
		public List<ActualStateRecordView> ActualStateRecords { get; set; }
		public List<TransferRecordView> TransferRecords { get; set; }
		public List<ComponentLLPCategoryChangeRecordView> ChangeLLPCategoryRecords { get; set; }
		public List<ComponentDirectiveView> ComponentDirectives { get; set; }
		public List<ComponentLLPCategoryDataView> LLPData { get; set; }


		public string Position
		{
			get { return TransferRecords.GetLast() != null ? TransferRecords.GetLast().Position : ""; }
		}

		public ComponentView(Component source)
		{
			Id = source.Id;
			AircaraftId = source.AircaraftId;
			PartNumber = source.PartNumber;
			ManufactureDate = source.ManufactureDate.Value;
			IsBaseComponent = source.IsBaseComponent;
			LLPMark = source.LLPMark;
			LLPCategories = source.LLPCategories;
			_threshold = new Threshold();
			Warranty = Lifelength.ConvertFromByteArray(source.Warranty);
			WarrantyNotify = Lifelength.ConvertFromByteArray(source.WarrantyNotify);
			LifeLimit = Lifelength.ConvertFromByteArray(source.LifeLimit);
			LifeLimitNotify = Lifelength.ConvertFromByteArray(source.LifeLimitNotify);
			ActualStateRecords = source.ActualStateRecords.Select(i => new ActualStateRecordView(i)).ToList();
			TransferRecords = source.TransferRecords.Select(i => new TransferRecordView(i)).ToList();
			ChangeLLPCategoryRecords = source.ChangeLLPCategoryRecords.Select(i => new ComponentLLPCategoryChangeRecordView(i)).ToList();
			ComponentDirectives = source.ComponentDirectives.Select(i => new ComponentDirectiveView(i)).ToList();
			LLPData = source.LLPData.Select(i => new ComponentLLPCategoryDataView(i)).ToList();
			AverageUtilization = AverageUtilization.ConvertFromByteArray(source.AverageUtilization);
		}

		public ComponentView()
		{
			
		}

		#region Implementation of IDirective

		public BaseView LifeLengthParent => this;

		private List<NextPerformance> _nextPerformances;
		private IThreshold _threshold;
		public List<NextPerformance> NextPerformances => _nextPerformances ?? (_nextPerformances = new List<NextPerformance>());

		public IThreshold Threshold
		{
			get => _threshold ?? (_threshold = new Threshold());
			set => _threshold = value;
		}

		public DirectiveRecordView LastPerformance => null;
		public bool IsClosed { get; set; }
		public void ResetMathData()
		{
			NextPerformances.Clear();
		}

		#endregion
	}
}