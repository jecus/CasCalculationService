using System;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ComponentLLPCategoryDataView : BaseView
	{
		public Lifelength LLPLifeLength { get; set; }

		public Lifelength LLPLifeLimit { get; set; }

		public Lifelength Notify { get; set; }

		public Lifelength LLPLifeLengthCurrent { get; set; }

		public Lifelength LLPLifeLengthForDate { get; set; }

		public DateTime? Date { get; set; }

		public ComponentLLPCategoryDataView(ComponentLLPCategoryData source)
		{
			Id = source.Id;
			LLPLifeLength = Lifelength.ConvertFromByteArray(source.LLPLifeLength);
			LLPLifeLimit = Lifelength.ConvertFromByteArray(source.LLPLifeLimit);
			Notify = Lifelength.ConvertFromByteArray(source.Notify);
			LLPLifeLengthCurrent = Lifelength.ConvertFromByteArray(source.LLPLifeLengthCurrent);
			LLPLifeLengthForDate = Lifelength.ConvertFromByteArray(source.LLPLifeLengthForDate);
			Date = source.Date;
		}
	}
}