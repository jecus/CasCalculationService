using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.Calculator.Views;
using BusinessLayer.Vendors;
using Entity;
using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Views
{
	public class BaseComponentView : ComponentView
	{
		public BaseComponentType BaseComponentTypeId { get; set; }

		public LifelengthCollection LifelengthCalculated { get; set; }

		public BaseComponentView(BaseComponent source)
		{
			Id = source.Id;
			AircaraftId = source.AircaraftId;
			PartNumber = source.PartNumber;
			ManufactureDate = source.ManufactureDate.Value;
			IsBaseComponent = source.IsBaseComponent;
			LLPMark = source.LLPMark;
			LLPCategories = source.LLPCategories;
			ActualStateRecords = source.ActualStateRecords.Select(i => new ActualStateRecordView(i)).ToList();
			TransferRecords = source.TransferRecords.Select(i => new TransferRecordView(i)).ToList();
			ChangeLLPCategoryRecords = source.ChangeLLPCategoryRecords.Select(i => new ComponentLLPCategoryChangeRecordView(i)).ToList();
			BaseComponentTypeId = source.BaseComponentTypeId;
		}
	}
}