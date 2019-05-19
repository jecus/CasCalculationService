using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entity.Entity
{
	[Table("Aircrafts", Schema = "dbo")]
	public class Aircraft : BaseEntity
	{
		[NotMapped]
		public int AircraftFrameId { get; set; }

		[Column("APUFH")]
		public double APUFH { get; set; }

		[Column("RegistrationNumber")]
		public string RegistrationNumber { get; set; }

		[Column("ManufactureDate")]
		public DateTime ManufactureDate { get; set; }

		[Column("ApuUtizationPerFlightinMinutes")]
		public short? ApuUtizationPerFlightinMinutes { get; set; }
	}
}
