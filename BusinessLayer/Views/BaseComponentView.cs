using System.Linq;
using BusinessLayer.Calculator.Views;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using Entity;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class BaseComponentView : ComponentView
	{
		private LifelengthCollection _lifelengthCalculated;
		public BaseComponentType BaseComponentTypeId { get; set; }

		public LifelengthCollection LifelengthCalculated
		{
			get => _lifelengthCalculated ?? (_lifelengthCalculated = new LifelengthCollection(ManufactureDate));
			set => _lifelengthCalculated = value;
		}

		public BaseComponentView(BaseComponent source)
		{
			if (source == null)
				return;
			Id = source.Id;
			AircaraftId = source.AircaraftId;
			PartNumber = source.PartNumber;
			ManufactureDate = source.ManufactureDate.Value;
			IsBaseComponent = source.IsBaseComponent;
			LLPMark = source.LLPMark;
			LLPCategories = source.LLPCategories;
			ActualStateRecords = source.ActualStateRecords?.Select(i => new ActualStateRecordView(i)).ToList();
			TransferRecords = source.TransferRecords?.Select(i => new TransferRecordView(i)).ToList();
			ChangeLLPCategoryRecords = source.ChangeLLPCategoryRecords?.Select(i => new ComponentLLPCategoryChangeRecordView(i)).ToList();
			BaseComponentTypeId = source.BaseComponentTypeId;
			AverageUtilization = AverageUtilization.ConvertFromByteArray(source.AverageUtilization);
		}
	}
}