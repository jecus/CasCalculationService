using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Models;

namespace Entity.Entity
{
	[Table("AircraftFlights", Schema = "dbo")]
	public class AircraftFlight : BaseEntity
	{
		[Column("AircraftId")]
		public int? AircraftId { get; set; }

		[Column("ATLBID")]
		public int ATLBID { get; set; }

		[Column("FlightDate")]
		public DateTime? FlightDate { get; set; }

		[Column("OutTime")]
		public int? OutTime { get; set; }

		[Column("InTime")]
		public int? InTime { get; set; }

		[Column("TakeOffTime")]
		public int TakeOffTime { get; set; }

		[Column("LDGTime")]
		public int LDGTime { get; set; }

		[Column("AtlbRecordType")]
		public AtlbRecordType AtlbRecordType { get; set; }

		[Column("Cycles")]
		public int Cycles { get; set; }

		[Column("CancelReasonId")]
		public int CancelReasonId { get; set; }

		[NotMapped]
		public List<RunUp> RunupsCollection { get; set; }

	}
}