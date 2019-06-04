using System;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class AircraftView : BaseView
	{
		public int AircraftFrameId { get; set; }

		public double APUFH { get; set; }

		public string RegistrationNumber { get; set; }

		public DateTime ManufactureDate { get; set; }

		public short? ApuUtizationPerFlightinMinutes { get; set; }

		public AircraftView(Aircraft source)
		{
			Id = source.Id;
			AircraftFrameId = source.AircraftFrameId;
			APUFH = source.APUFH;
			RegistrationNumber = source.RegistrationNumber;
			ManufactureDate = source.ManufactureDate;
			ApuUtizationPerFlightinMinutes = source.ApuUtizationPerFlightinMinutes;
		}
	}
}