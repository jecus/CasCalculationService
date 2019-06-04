using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Views
{
	public class AircraftFlightView : BaseView
	{
		public int? AircraftId { get; set; }

		public int ATLBID { get; set; }

		public DateTime FlightDate { get; set; }

		public int? OutTime { get; set; }

		public int? InTime { get; set; }

		public int TakeOffTime { get; set; }

		public int LDGTime { get; set; }

		public AtlbRecordType AtlbRecordType { get; set; }

		public int Cycles { get; set; }

		public Reason CancelReason { get; set; }

		public List<RunUpView> RunupsCollection { get; set; }

		#region public Lifelength FlightTimeLifelength

		/// <summary> 
		/// Возвращает наработку на основе FlightTime(Ldg - TakeOff) за заданный полет
		/// </summary>
		
		public Lifelength FlightTimeLifelength
		{
			get { return new Lifelength(null, Cycles, FlightTimeTotalMinutes); }
		}

		#endregion

		#region public int FlightTimeTotalMinutes

		/// <summary>
		/// Возвращает время полета воздушного судна
		/// </summary>
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

		public AircraftFlightView(AircraftFlight source)
		{
			Id = source.Id;
			AircraftId = source.AircraftId;
			ATLBID = source.ATLBID;
			FlightDate = source.FlightDate.Value;
			OutTime = source.OutTime;
			InTime = source.InTime;
			TakeOffTime = source.TakeOffTime;
			LDGTime = source.LDGTime;
			AtlbRecordType = source.AtlbRecordType;
			Cycles = source.Cycles;
			RunupsCollection = source.RunupsCollection.Select(i => new RunUpView(i));
		}
	}
}