using System;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Calculator.Dictionaries;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class EngineTimeInRegimeView : BaseView
	{
		public int FlightId { get; set; }

		public int? BaseComponentId { get; set; }
		public BaseComponentView BaseComponent{ get; set; }

		public int? FlightRegimeId { get; set; }
		public FlightRegime FlightRegime { get; set; }

		public TimeSpan TimeInRegime { get; set; }

		public DateTime RecordDate { get; set; }

		public short? GroundAir { get; set; }

		public EngineTimeInRegimeView(EngineTimeInRegime source)
		{
			Id = source.Id;
			FlightId = source.FlightId;
			BaseComponentId = source.EngineId;
			FlightRegimeId = source.FlightRegimeId;
			TimeInRegime =new TimeSpan(0 , source.TimeInRegime.HasValue ? source.TimeInRegime.Value : 0, 0); 
			RecordDate = source.RecordDate;
			GroundAir = source.GroundAir;
			FlightRegime = FlightRegime.GetItemById(source.FlightRegimeId.HasValue ? source.FlightRegimeId.Value : -1);
			BaseComponent = new BaseComponentView(source.Component);
		}
	}
}