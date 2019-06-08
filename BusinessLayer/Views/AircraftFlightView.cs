using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Vendors;
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

		public ReasonView CancelReason { get; set; }

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

		public List<EngineTimeInRegimeView> PowerUnitTimeInRegimeCollection { get; set; }

		#endregion

		#region public TimeSpan FlightTime
		/// <summary>
		/// Время полета ВС по Takeoff-LDG
		/// </summary>
		public TimeSpan FlightTime
		{
			get
			{
				int flightTime = LDGTime - TakeOffTime;
				if (flightTime < 0) flightTime += 24 * 60;

				TimeSpan time = new TimeSpan(flightTime / 60, flightTime - (flightTime / 60) * 60, 0);

				return time;
			}
		}
		#endregion

		#region public Int32 BlockTimeTotalMinutes { get; }
		/// <summary>
		/// Возвращает время полета ВС по BlockTime(Out-In)
		/// </summary>

		public Int32 BlockTimeTotalMinutes
		{
			get
			{
				int blockTime = (int) (InTime - OutTime);
				if (blockTime < 0) blockTime += 24 * 60;
				return blockTime;
			}
		}

		#endregion

		#region public Lifelength BlockTimeLifelenght { get; }
		/// <summary> 
		/// Возвращает наработку на основе BlockTime(In - Out) за заданный полет
		/// </summary>
		public Lifelength BlockTimeLifelenght
		{
			get { return new Lifelength(null, Cycles, BlockTimeTotalMinutes); }
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
			RunupsCollection = source.RunupsCollection.Select(i => new RunUpView(i)).ToList();
			PowerUnitTimeInRegimeCollection = source.Regimes.Select(i => new EngineTimeInRegimeView(i)).ToList();
			CancelReason = new ReasonView(source.CancelReason);
		}
	}
}