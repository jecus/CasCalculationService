using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator.Views;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentView : BaseView
	{
		public int AircaraftId { get; set; }

		public string PartNumber { get; set; }

		public DateTime ManufactureDate { get; set; }

		public bool IsBaseComponent { get; set; }

		public List<ActualStateRecordView> ActualStateRecords { get; set; }
		public List<TransferRecordView> TransferRecords { get; set; }
		public List<ComponentLLPCategoryChangeRecordView> ChangeLLPCategoryRecords { get; set; }


		public string Position
		{
			get { return /*TransferRecords.GetLast() != null ? TransferRecords.GetLast().Position :*/ ""; }
		}

		public ComponentView(Component source)
		{
			Id = source.Id;
			AircaraftId = source.AircaraftId;
			PartNumber = source.PartNumber;
			ManufactureDate = source.ManufactureDate.Value;
			IsBaseComponent = source.IsBaseComponent;
			ActualStateRecords = source.ActualStateRecords.Select(i => new ActualStateRecordView(i)).ToList();
			TransferRecords = source.TransferRecords.Select(i => new TransferRecordView(i)).ToList();
			ChangeLLPCategoryRecords = source.ChangeLLPCategoryRecords.Select(i => new ComponentLLPCategoryChangeRecordView(i)).ToList();

		}

		public ComponentView()
		{
			
		}
	}
}