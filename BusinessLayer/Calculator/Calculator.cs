using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Repositiries;
using Entity;
using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Calculator
{
	public class Calculator : ICalculator
	{
		private readonly IAircraftFlightRepository _aircraftFlightRepository;
		private readonly IAircraftRepository _aircraftRepository;
		private readonly IComponentRepository _componentRepository;

		public Calculator(IAircraftFlightRepository aircraftFlightRepository,
			IAircraftRepository aircraftRepository,
			IComponentRepository componentRepository)
		{
			_aircraftFlightRepository = aircraftFlightRepository;
			_aircraftRepository = aircraftRepository;
			_componentRepository = componentRepository;
		}


		// Воздушное судно

		public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime date)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthOnStartOfDay(aircraft, date);
		}

		public async Task<Lifelength> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime date)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthOnEndOfDay(aircraft, DateTime.Today);
		}

		public async Task<Lifelength> GetFlightLifelengthForPeriod(int aircraftId, DateTime fromDate, DateTime toDate)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthForPeriod(aircraft, fromDate, toDate);
		}

		public async Task<Lifelength> GetFlightLifelengthIncludingThisFlight(int flightId)
		{
			var flight = await _aircraftFlightRepository.GetAircraftFlightsByIdAsync(flightId);
			return await getFlightLifelengthIncludingThisFlight(flight);
		}

		#region public async Task<Lifelength> GetCurrentFlightLifelength(int aircraftId)
		/// <summary>
		/// Возвращает текущий налет воздушного судна
		/// </summary>
		/// <param name="aircraft"></param>
		/// <returns></returns>
		public async Task<Lifelength> GetCurrentFlightLifelength(int aircraftId)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthOnEndOfDay(aircraft, DateTime.Today);
		}
		#endregion

		#region private async Task<Lifelength> getFlightLifelengthIncludingThisFlight(AircraftFlight flight)
		/// <summary> 
		/// Возвращает налет воздушного судна после завершения указанного полета
		/// </summary>
		/// <param name="flight"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthIncludingThisFlight(AircraftFlight flight)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(flight.AircraftId.Value);
			if (aircraft == null) throw new Exception($"Flight {flight.Id} has no aircraft related");

			// Если это был последний полет за указанный день возвращаем налет вс на конец дня
			if (flight.LDGTime < flight.TakeOffTime) return await getFlightLifelengthOnEndOfDay(aircraft, flight.FlightDate.Value);

			// Сначала получаем налет воздушного судна на заданную дату 
			var initial = await getFlightLifelengthOnStartOfDay(aircraft, flight.FlightDate.Value);

			// Пробегаемся по всем полетам, которые идут до этого полета
			var flights = await _aircraftFlightRepository.GetAircraftFlightsByAircraftIdAsync(aircraft.Id);
			foreach (var t in flights.Where(f => f.AtlbRecordType != AtlbRecordType.Maintenance))
			{
				if (t.FlightDate.Value.Date == flight.FlightDate.Value.Date)
					initial.Add(t.FlightTimeLifelength);

				// возвращаемся если дошли до заданного полета
				if (t.Id == flight.Id) break;
			}

			// Календарь
			initial.Days = GetDays(aircraft.ManufactureDate, (DateTime) flight.FlightDate);

			// initial хранит в себе налет вс на начало соответсвующего дня + все полеты по заданный включительно
			return initial;
		}
		#endregion
		
		#region private Lifelength getFlightLifelengthOnStartOfDay(Aircraft aircraft, DateTime date)

		/// <summary>
		/// Возвращает налет воздушного судна на начало дня (без учета совершенных полетов)
		/// </summary>
		/// <param name="aircraft"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthOnStartOfDay(Aircraft aircraft, DateTime date)
		{
			// ресурс на момент производства равен нулю 
			date = date.Date;
			if (date <= aircraft.ManufactureDate) return Lifelength.Zero;

			var aircraftFrame = await _componentRepository.GetBaseComponentByIdAsync(aircraft.AircraftFrameId);
			// вычисляем результат
			Lifelength res;

			// если на указанную дату было задано актуальное состояние - оно и будет является налетом воздушного судна на начало дня
			var actual = aircraftFrame.ActualStateRecords.GetLastKnownRecord(date);
			if (actual != null && actual.RecordDate.Value.Date == date)
			{
				res = new Lifelength(Lifelength.ConvertFromByteArray(actual.OnLifelength));

				// Если мы не имеем один из параметров актуального состояния - берем его из наработки на предыдущий день
				if (res.Cycles == null || res.TotalMinutes == null)
					res.CompleteNullParameters(await getFlightLifelengthOnEndOfDay(aircraft, actual.RecordDate.Value.Date.AddDays(-1)));

				// Выставляем правильный календарь
				res.Days = GetDays(aircraft.ManufactureDate, date);
			}
			else
			{
				// если актуальных состояние вообще не задано либо задано но на давнюю дату то 
				// налет вс на начало дня равен налету вс на конец предыдущего дня
				res = await getFlightLifelengthOnEndOfDay(aircraft, date.AddDays(-1));
			}

			return new Lifelength(res);
		}

		#endregion

		#region private Lifelength getFlightLifelengthOnEndOfDay(Aircraft aircraft, DateTime date)
		/// <summary>
		/// Возвращает налет воздушного судна на конец дня (учитывая совершенные полеты)
		/// </summary>
		/// <param name="aircraft"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthOnEndOfDay(Aircraft aircraft, DateTime date)
		{
			// если склад то свойство aircraft пусто, а наработка равна нулю
			if (aircraft == null) return Lifelength.Zero;

			// раньше производства равен нулю 
			date = date.Date;
			if (date < aircraft.ManufactureDate) return Lifelength.Zero;

			var aircraftFrame = await _componentRepository.GetBaseComponentByIdAsync(aircraft.AircraftFrameId);

			// вычисляем результат

			// получаем последнее Actual State на заданную дату 
			// получаем на него Opening Lifelength и затем прибавляем все сделанные полеты за интервал
			var actual = aircraftFrame.ActualStateRecords.GetLastKnownRecord(date);
			var res = (actual == null) ? Lifelength.Zero : await getFlightLifelengthOnStartOfDay(aircraft, actual.RecordDate.Value.Date);
			var startDate = (actual == null) ? aircraft.ManufactureDate : actual.RecordDate.Value.Date;
			res.Add(await getFlightsLifelengthByPeriod(aircraft, startDate, date)); // opening сейчас учитывает все полеты от start date

			//TODO (Evgenii Babak) : Посмотреть почему добавляем + 1 день
			// календарь
			res.Days = GetDays(aircraft.ManufactureDate, date) + 1;

			return new Lifelength(res);
		}

		#endregion

		#region private Lifelength getFlightsLifelengthByPeriod(Aircraft aircraft, DateTime dateFrom, DateTime dateTo)
		/// <summary>
		/// Возвращает суммарный налет воздушного судна за указанный период
		/// </summary>
		/// <param name="aircraft"></param>
		/// <param name="dateFrom"></param>
		/// <param name="dateTo"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightsLifelengthByPeriod(Aircraft aircraft, DateTime dateFrom, DateTime dateTo)
		{
			dateFrom = dateFrom.Date;
			dateTo = dateTo.Date;

			var flights = await _aircraftFlightRepository.GetAircraftFlightsByAircraftIdAsync(aircraft.Id);

			var aircraftFrame = await _componentRepository.GetBaseComponentByIdAsync(aircraft.AircraftFrameId);
			// пробегаемся по всем полетам
			var res = await getFlightLifelengthByPeriod(flights, aircraftFrame, dateFrom, dateTo);

			res.Days = GetDays(dateFrom, dateTo);
			return res;
		}
		#endregion

		#region private Lifelength getFlightLifelengthForPeriod(Aircraft aircraft, DateTime fromDate, DateTime toDate)
		/// <summary>
		/// Возвращает налет воздушного судна за заданный интервал
		/// </summary>
		/// <param name="aircraft"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthForPeriod(Aircraft aircraft, DateTime fromDate, DateTime toDate)
		{
			// Налет воздушного судна между двумя датами равен разности налета на заданные даты
			var res = await getFlightLifelengthOnEndOfDay(aircraft, toDate);
			res.Substract(await getFlightLifelengthOnEndOfDay(aircraft, fromDate));
			res.Days = Convert.ToInt32((toDate.Date.Ticks - fromDate.Date.Ticks) / TimeSpan.TicksPerDay);
			return res;
		}
		#endregion

		//AircraftFlights

		#region private Lifelength getFlightLifelengthByPeriod(AircraftFlightCollection flights, BaseComponent bd, DateTime dateFrom, DateTime dateTo)

		/// <summary>
		/// возвращает наработку детали за указанный период, если есть соответствующие данные в полетах.
		/// <br/>
		/// Для Engine и APU наработка расчитывается по Runup-ам
		/// <br/>
		/// Для Propellers наработка насчитывается по RunUp-ам двигателей, имеющих туже позицию
		/// <br/> 
		/// Для Frame и LG наработка расчитывается по полетам, имеющим ATLBRecordType = Flight и CancelReason = "N/A"
		/// </summary>
		/// <param name="flights"></param>
		/// <param name="bd">Базовая деталь, для которой необходимо расчитать наработку</param>
		/// <param name="dateFrom">Начало интервала</param>
		/// <param name="dateTo">Конец интервала</param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthByPeriod(List<AircraftFlight> flights, Component bd, DateTime dateFrom, DateTime dateTo)
		{
			var res = Lifelength.Zero;
			if (bd == null) return res;

			foreach (var flight in flights.Where(f => f.AtlbRecordType != AtlbRecordType.Maintenance))
			{
				if (flight.FlightDate.Value.Date >= dateFrom && flight.FlightDate.Value.Date <= dateTo)
				{
					if (bd.BaseComponentTypeId == BaseComponentType.APU)
					{
						var aircraft = await _aircraftRepository.GetAircraftByIdAsync(bd.AircaraftId);
						res.Add(new Lifelength(0, 1, (int?)(flight.FlightTimeLifelength.TotalMinutes * aircraft.APUFH)));
					}
					else res.Add(await getFlightLifelength(flight, bd));
				}
			}
			return res;
		}
		#endregion

		#region private Lifelength getFlightLifelength(AircraftFlight flight, BaseComponent bd)
		/// <summary>
		/// возвращает наработку Базового агрегата за данный полет
		/// Применимо для любых типов Базовых агрегатов 
		/// </summary>
		/// <param name="bd"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelength(AircraftFlight flight, Component bd)
		{
			if (bd == null)
				return Lifelength.Zero;

			if ((bd.BaseComponentTypeId == BaseComponentType.Frame || bd.BaseComponentTypeId == BaseComponentType.LandingGear))
			{
				if (flight.AtlbRecordType == AtlbRecordType.Flight /*&& flight.CancelReason == null*/)
					return flight.FlightTimeLifelength;

				return Lifelength.Zero;
			}

			if (bd.BaseComponentTypeId != BaseComponentType.Engine &&
				bd.BaseComponentTypeId != BaseComponentType.APU &&
				bd.BaseComponentTypeId != BaseComponentType.Propeller &&
				flight.RunupsCollection.Count == 0)
				return Lifelength.Zero;

			var res = Lifelength.Zero;
			List<RunUp> runs = null;

			if (bd.BaseComponentTypeId == BaseComponentType.Engine || bd.BaseComponentTypeId == BaseComponentType.APU)
				runs = flight.RunupsCollection/*GetByBaseComponent(bd)*/.ToList();
			else if (bd.BaseComponentTypeId == BaseComponentType.Propeller)
				runs = flight.RunupsCollection.Where(r => r.BaseComponent.BaseComponentTypeId == BaseComponentType.Engine &&
													r.BaseComponent.Position.Trim() == bd.Position.Trim()).ToList();

			if (runs != null)
			{
				if (runs.Count == 0 && bd.BaseComponentTypeId != BaseComponentType.APU)
					return flight.FlightTimeLifelength;
				if (runs.Count == 0 && bd.BaseComponentTypeId == BaseComponentType.APU)
				{
					var parentAircraft = await _aircraftRepository.GetAircraftByIdAsync(bd.AircaraftId);
					if (parentAircraft.ApuUtizationPerFlightinMinutes != null)
						return new Lifelength(null, 1, parentAircraft.ApuUtizationPerFlightinMinutes);
				}

				foreach (var t in runs)
					res.Add(t.Lifelength);

			}
			return res;
		}
		#endregion


		/*
         * Дополнительно
         */
		#region public static int GetDays(DateTime dateFrom, DateTime dateTo)
		/// <summary>
		/// Возвращает количество дней между двумя датами 
		/// </summary>
		/// <param name="dateFrom"></param>
		/// <param name="dateTo"></param>
		/// <returns></returns>
		public static int GetDays(DateTime dateFrom, DateTime dateTo)
		{
			return Convert.ToInt32((dateTo.Date.Ticks - dateFrom.Date.Ticks) / TimeSpan.TicksPerDay);
		}
		#endregion

	}
}
