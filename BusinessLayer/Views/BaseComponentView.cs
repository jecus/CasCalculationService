using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator.Views;
using Entity;
using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Views
{
	public class BaseComponentView : ComponentView
	{
		public BaseComponentType BaseComponentTypeId { get; set; }

		public List<Lifelength> LifelengthCalculated { get; set; }

		public BaseComponentView(BaseComponent source)
		{
			Id = source.Id;
			AircaraftId = source.AircaraftId;
			PartNumber = source.PartNumber;
			ManufactureDate = source.ManufactureDate.Value;
			IsBaseComponent = source.IsBaseComponent;
			ActualStateRecords = source.ActualStateRecords.Select(i => new ActualStateRecordView(i)).ToList();
			TransferRecords = source.TransferRecords.Select(i => new TransferRecordView(i)).ToList();
			ChangeLLPCategoryRecords = source.ChangeLLPCategoryRecords.Select(i => new ComponentLLPCategoryChangeRecordView(i)).ToList();
			LifelengthCalculated = source.LifelengthCalculated;
			BaseComponentTypeId = source.BaseComponentTypeId;

		}
	}
}