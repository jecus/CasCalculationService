using System;
using BusinessLayer.Views;
using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Calculator.Views
{
	public class ActualStateRecordView : BaseView
	{
		public int FlightRegimeId { get; set; }

		public string Remarks { get; set; }

		public Lifelength OnLifelength {get; set; } 
		
		public DateTime? RecordDate { get; set; }

		public int? WorkRegimeTypeId { get; set; }

		public int ComponentId { get; set; }

		public ActualStateRecordView(ActualStateRecord source)
		{
			Id = source.Id;
			FlightRegimeId = source.FlightRegimeId;
			Remarks = source.Remarks;
			OnLifelength = Lifelength.ConvertFromByteArray(source.OnLifelengthByte);
			RecordDate = source.RecordDate;
			WorkRegimeTypeId = source.WorkRegimeTypeId;
			ComponentId = source.ComponentId.Value;
		}
	}
}