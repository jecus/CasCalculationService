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

		[NotMapped]
		public List<RunUp> RunupsCollection { get; set; }

		#region public Lifelength FlightTimeLifelength

		/// <summary> 
		/// Возвращает наработку на основе FlightTime(Ldg - TakeOff) за заданный полет
		/// </summary>
		[NotMapped]
		public Lifelength FlightTimeLifelength
		{
			get { return new Lifelength(null, Cycles, FlightTimeTotalMinutes); }
		}

		#endregion

		#region public int FlightTimeTotalMinutes

		/// <summary>
		/// Возвращает время полета воздушного судна
		/// </summary>
		[NotMapped]
			public int FlightTimeTotalMinutes
			{
				get
				{
					int x = LDGTime - TakeOffTime;
					if (x < 0) x += 24 * 60;
					return x;
				}
			}

			#endregion

	}
}