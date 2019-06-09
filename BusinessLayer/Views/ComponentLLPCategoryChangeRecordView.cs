using System;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentLLPCategoryChangeRecordView : BaseView
	{
		public int? ParentId { get; set; }

		public DateTime RecordDate { get; set; }

		public Lifelength OnLifeLength { get; set; }
	

		public ComponentLLPCategoryChangeRecordView(ComponentLLPCategoryChangeRecord source)
		{
			if (source == null)
				return;
			Id = source.Id;
			ParentId = source.ParentId;
			RecordDate = source.RecordDate.Value;
			OnLifeLength = Lifelength.ConvertFromByteArray(source.OnLifeLengthByte);
		}
		
	}
}