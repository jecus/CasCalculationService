﻿using System;
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

		#region public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime date)

		public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime date)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthOnStartOfDay(aircraft, date);
		}


		#endregion

		#region public async Task<Lifelength> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime date)

		public async Task<Lifelength> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime date)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthOnEndOfDay(aircraft, DateTime.Today);
		}

		#endregion

		#region public async Task<Lifelength> GetFlightLifelengthForPeriod(int aircraftId, DateTime fromDate, DateTime toDate)

		public async Task<Lifelength> GetFlightLifelengthForPeriod(int aircraftId, DateTime fromDate, DateTime toDate)
		{
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(aircraftId);
			return await getFlightLifelengthForPeriod(aircraft, fromDate, toDate);
		}

		#endregion

		#region public async Task<Lifelength> GetFlightLifelengthIncludingThisFlight(int flightId)

		public async Task<Lifelength> GetFlightLifelengthIncludingThisFlight(int flightId)
		{
			var flight = await _aircraftFlightRepository.GetAircraftFlightsByIdAsync(flightId);
			return await getFlightLifelengthIncludingThisFlight(flight);
		}

		#endregion

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

		#region private async Task<Lifelength> getFlightLifelengthOnStartOfDay(Aircraft aircraft, DateTime date)

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
				res = new Lifelength(actual.OnLifelength);

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

		#region private async Task<Lifelength> getFlightLifelengthOnEndOfDay(Aircraft aircraft, DateTime date)
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

		#region private async Task<Lifelength> getFlightsLifelengthByPeriod(Aircraft aircraft, DateTime dateFrom, DateTime dateTo)
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

		#region private async Task<Lifelength> getFlightLifelengthForPeriod(Aircraft aircraft, DateTime fromDate, DateTime toDate)
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

		// Базовый агрегат

		#region public async Task<Lifelength> GetFlightLifelengthIncludingThisFlight(BaseComponent baseComponent, AircraftFlight flight)

		/// <summary> 
		/// Возвращает налет базового агрегата после совершенного полета
		/// </summary>
		/// <param name="baseComponent"></param>
		/// <param name="flight"></param>
		/// <returns></returns>
		public async Task<Lifelength> GetFlightLifelengthIncludingThisFlight(BaseComponent baseComponent, AircraftFlight flight)
		{
			if (baseComponent == null) throw new Exception($"Flight {flight.Id} has no base component related");
			var aircraft = await _aircraftRepository.GetAircraftByIdAsync(baseComponent.AircaraftId);
			if (aircraft == null) throw new Exception($"Flight {flight.Id} has no aircraft related");

			// Если это был последний полет за указанный день возвращаем налет вс на конец дня
			if (flight.LDGTime < flight.TakeOffTime) return getFlightLifelengthOnEndOfDay(baseComponent, flight.FlightDate.Value);

			// Сначала получаем налет базовой детали, полученный днем ранее
			var initial = await getFlightLifelengthOnEndOfDay(baseComponent, flight.FlightDate.Value.Date.AddDays(-1));
			//TODO:(Evgenii Babak) Заметка для бага 598. Требуется учитывать актуальное состояние в этом методе для разрешения бага
			//без учета актуального состояния initial будет LifeLenght.Zero если даты полета и установки базового агрегата совпадают
			//требуется стори для изменения подхода в расчете наработки бд. Нужно избавиться от вычитания одного дня
			var fl = await _aircraftFlightRepository.GetAircraftFlightsOnDate(aircraft.Id, flight.FlightDate.Value.Date);
			var flights = fl.Where(f => f.AtlbRecordType != AtlbRecordType.Maintenance);

			// Пробегаемся по всем полетам, которые идут до этого полета
			foreach (var t in flights)
			{
				var add = await getFlightLifelength(t, baseComponent);
				if (baseComponent.BaseComponentTypeId == BaseComponentType.APU)
				{
					initial.Add(new Lifelength(0, 1, (int?)(t.FlightTimeLifelength.TotalMinutes * aircraft.APUFH)));
				}
				else initial.Add(add);

				// возвращаемся если дошли до заданного полета
				if (t.Id == flight.Id) break;
			}

			// Календарь
			initial.Days = GetDays(baseComponent.ManufactureDate.Value, flight.FlightDate.Value);

			// initial хранит в себе налет вс на начало соответсвующего дня + все полеты по заданный включительно
			return initial;
		}
		#endregion

		#region public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseComponent baseComponent, DateTime date)

		/// <summary>
		/// Возвращает наработку базового агрегата на начало дня - без учета полетов воздушного судна и перемещений
		/// </summary>
		/// <param name="baseComponent"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseComponent baseComponent, DateTime date)
		{
			return await getFlightLifelengthOnStartOfDay(baseComponent, date);
		}

		#endregion

		#region public Lifelength GetFlightLifelengthOnStartOfDay(BaseComponent baseComponent, DateTime date, FlightRegime flightRegime)

		/// <summary>
		/// Возвращает работу базового агрегата на начало дня в заданном режиме. (только для Engine, Propellers, APU)
		/// </summary>
		/// <param name="baseComponent">базовый агрегат></param>
		/// <param name="date">Дата, на начало которой необходимо вернуть наработку</param>
		/// <param name="flightRegime">режим работы агрегата</param>
		/// <returns></returns>
		public async Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseComponent baseComponent, DateTime date, FlightRegime flightRegime)
		{
			//наработка по режимам работы расчитывается только для Двигателей, Пропеллеров и ВСУ
			//по другим типам деталей наработка по режимам не ведется
			if (baseComponent.BaseComponentTypeId != BaseComponentType.APU &&
				baseComponent.BaseComponentTypeId != BaseComponentType.Engine &&
				baseComponent.BaseComponentTypeId != BaseComponentType.Propeller)
			{
				return await getFlightLifelengthOnStartOfDay(baseComponent, date);
			}

			if (flightRegime == null)
				flightRegime = FlightRegime.UNK;
			if (flightRegime == FlightRegime.UNK)
			{
				//при режиме UNK возвращается наработка базовой детали во всех режимах
				return await getFlightLifelengthOnStartOfDay(baseComponent, date);
			}

			// ресурс на момент производства равен нулю
			date = date.Date;
			if (date <= baseComponent.ManufactureDate) return Lifelength.Zero;

			// если наработка уже подсчитана - возвращаем ее
			var saved = baseComponent.LifelengthCalculated.GetLifelengthOnStartOfDay(date, flightRegime);
			if (saved != null) return new Lifelength(saved);

			// вычисляем результат
			var res = getFlightLifelengthOnEndOfDay(baseComponent, date.AddDays(-1), flightRegime);

			return new Lifelength(res);
		}

		#endregion

		#region private async Task<Lifelength> getFlightLifelengthOnEndOfDay(BaseComponent baseComponent, DateTime effectiveDate)
		/// <summary>
		/// Возваращает наработку базового агрегата на конец заданной даты
		/// </summary>
		/// <param name="baseComponent"></param>
		/// <param name="effectiveDate"></param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthOnEndOfDay(BaseComponent baseComponent, DateTime effectiveDate)
		{
			if (baseComponent == null) return Lifelength.Null;
			// Сначала загружаем математический аппарат
			Init();
			
			// Если это Frame воздушного судна, то возвращаем налет самого воздушного судна
			if (baseComponent.BaseComponentTypeId == BaseComponentType.Frame)
			{
				var parentAircraft = await _aircraftRepository.GetAircraftByIdAsync(baseComponent.AircaraftId);
				return await getFlightLifelengthOnEndOfDay(parentAircraft, effectiveDate);
			}

			// Возвращаем ноль на все, что раньше даты производства
			if (effectiveDate < baseComponent.ManufactureDate) return Lifelength.Zero;

			// Наработка базового агрегата на заданную дату считается 
			// от момента последнего актуального состояния и дальше суммируя налеты ВС между перемещениями базового агрегата
			var res = Lifelength.Zero;

			var actualState = baseComponent.ActualStateRecords.GetLastKnownRecord(effectiveDate);

			var llpRecord = baseComponent.ChangeLLPCategoryRecords.GetLast();

			if (llpRecord != null && actualState != null)
			{
				res = new Lifelength(actualState.OnLifelength);
			}
			else if (llpRecord != null)
				res = new Lifelength(llpRecord.OnLifeLength);
			else if (actualState != null)
				res = new Lifelength(actualState.OnLifelength);

			// Если мы не имеем один из параметров актуального состояния - берем его из наработки на предыдущий день
			if (actualState != null && (actualState.OnLifelength.Cycles == null || actualState.OnLifelength.TotalMinutes == null))
				res.CompleteNullParameters(await getFlightLifelengthOnEndOfDay(baseComponent, actualState.RecordDate.Value.Date.AddDays(-1)));
			var transfers = (actualState != null) ? baseComponent.TransferRecords.GetRecords(actualState.RecordDate.Value.Date, effectiveDate) : baseComponent.TransferRecords.GetRecords(effectiveDate);

			// Суммируем интервалы между перемещениями
			if (transfers != null)
				for (int i = 0; i < transfers.Length; i++)
				{
					var a = transfers[i].DestinationObjectType == SmartCoreType.Aircraft ? _aircraftRepository.GetAircraftByIdAsync(transfers[i].DestinationObjectId.Value) : null;
					if (a == null) continue; // агрегат был помещен на склад, а склады не содержатся в коллекции воздушных судов

					// в середине цикла берем дату перемещения, а в начале берем дату актуального состояния 
					var dateFrom = i > 0 || actualState == null ? transfers[i].TransferDate : actualState.RecordDate.Value.Date;
					// в конце берем дату dateTo, а в середине цикла дату следующего перемещения
					var dateTo = i < transfers.Length - 1 ? transfers[i + 1].TransferDate : effectiveDate;

					//если дата установки и dateTo одинаковы
					if ((dateFrom == dateTo) && (dateTo == transfers[i].TransferDate))
						continue;
					// суммируем 
					//Lifelength delta = GetLifelength(a, dateFrom, dateTo);

					var delta = Lifelength.Zero;
					var flights = await _aircraftFlightRepository.GetAircraftFlightsByAircraftIdAsync(a.Id);
					if (baseComponent.BaseComponentTypeId == BaseComponentType.LandingGear)
						delta = await getFlightLifelengthForPeriod(a, dateFrom.Value, dateTo.Value);
					else if (baseComponent.BaseComponentTypeId == BaseComponentType.Propeller || baseComponent.BaseComponentTypeId == BaseComponentType.Engine)
					{

						var bdFlightLL = await getFlightLifelengthByPeriod(flights, baseComponent, dateFrom.Value, dateTo.Value);
						var aircraftFlightLL = await getFlightLifelengthForPeriod(a, dateFrom.Value, dateTo.Value);

						if (aircraftFlightLL.Cycles.HasValue && bdFlightLL.Cycles.HasValue && ((float)bdFlightLL.Cycles / (float)aircraftFlightLL.Cycles < 0.95) ||
							aircraftFlightLL.Hours.HasValue && bdFlightLL.Hours.HasValue && ((float)bdFlightLL.Hours / (float)aircraftFlightLL.Hours < 0.95))
							delta = aircraftFlightLL;
						else delta = bdFlightLL;
					}
					else delta = await getFlightLifelengthByPeriod(flights, baseComponent, dateFrom.Value, dateTo.Value);

					res.Add(delta);
				}

			//
			res.Days = GetDays(baseComponent.ManufactureDate.Value, effectiveDate); // +1 т.к. вторая граница интервала включительно

			// Сохраняем результат
			baseComponent.LifelengthCalculated.SetLifelengthOnEndOfDay(effectiveDate, res);
			//
			return new Lifelength(res);
		}
		#endregion

		#region private Lifelength getFlightLifelengthOnEndOfDay(BaseComponent baseComponent, DateTime effectiveDate, FlightRegime flightRegime)

		/// <summary>
		/// Возваращает работу базового агрегата в заданном режиме на конец заданной даты 
		/// <br/>(только для Engine, Propellers, APU)
		/// <br/>для агретатов другого типа вернет полную наработку
		/// </summary>
		/// <param name="baseComponent">базовый агрегат></param>
		/// <param name="effectiveDate">дата, на которую необходимо вернуть наработку</param>
		/// <param name="flightRegime">режим работы агрегата</param>
		/// <returns></returns>
		private async Task<Lifelength> getFlightLifelengthOnEndOfDay(BaseComponent baseComponent, DateTime effectiveDate, FlightRegime flightRegime)
		{
			if (baseComponent == null) return Lifelength.Null;
			//наработка по режимам работы расчитывается только для Двигателей, Пропеллеров и ВСУ
			//по другим типам деталей наработка по режимам не ведется
			if (baseComponent.BaseComponentTypeId != BaseComponentType.APU &&
				baseComponent.BaseComponentTypeId != BaseComponentType.Engine &&
				baseComponent.BaseComponentTypeId != BaseComponentType.Propeller)
			{
				return getFlightLifelengthOnStartOfDay(baseComponent, effectiveDate);
			}

			if (flightRegime == null)
				flightRegime = FlightRegime.UNK;
			if (flightRegime == FlightRegime.UNK)
			{
				//при режиме UNK возвращается наработка базовой детали во всех режимах
				return getFlightLifelengthOnStartOfDay(baseComponent, effectiveDate);
			}

			// Загрузка математического аппарата
			Init();
			// Возвращает ноль на все, что раньше даты производства
			if (effectiveDate < baseComponent.ManufactureDate) return Lifelength.Zero;

			// Наработка базового агрегата на заданную дату считается 
			// от момента последнего актуального состояния и дальше суммируя налеты ВС между перемещениями базового агрегата

			var actualState = baseComponent.ActualStateRecords.GetLastKnownRecord(effectiveDate, flightRegime);
			var res = (actualState != null) ? new Lifelength(actualState.OnLifelength) : Lifelength.Zero;
			// Если мы не имеем один из параметров актуального состояния - берем его из наработки на предыдущий день
			if (actualState != null && (actualState.OnLifelength.Cycles == null || actualState.OnLifelength.TotalMinutes == null))
				res.CompleteNullParameters(await getFlightLifelengthOnEndOfDay(baseComponent, actualState.RecordDate.Value.Date.AddDays(-1), flightRegime));
			var transfers = (actualState != null) ? baseComponent.TransferRecords.GetRecords(actualState.RecordDate.Value.Date, effectiveDate) : baseComponent.TransferRecords.GetRecords(effectiveDate);

			// Суммирование наработки между перемещениями
			if (transfers != null)
				for (int i = 0; i < transfers.Length; i++)
				{
					var a = transfers[i].DestinationObjectType == SmartCoreType.Aircraft ? _aircraftRepository.GetAircraftByIdAsync(transfers[i].DestinationObjectId) : null;
					if (a == null) continue; // агрегат был помещен на склад, а склады не содержатся в коллекции воздушных судов

					// в середине цикла берем дату перемещения, а в начале берем дату актуального состояния 
					var dateFrom = i > 0 || actualState == null ? transfers[i].TransferDate : actualState.RecordDate.Value.Date;
					// в конце берем дату dateTo, а в середине цикла дату следующего перемещения
					var dateTo = i < transfers.Length - 1 ? transfers[i + 1].TransferDate : effectiveDate;

					//если дата установки и dateTo одинаковы
					if ((dateFrom == dateTo) && (dateTo == transfers[i].TransferDate))
						continue;

					var flights = _aircraftFlightRepository.GetAircraftFlightsByAircraftIdAsync(a.Id);

					// суммируем 
					var delta = await getFlightLifelengthForPeriod(flights, baseComponent, dateFrom, dateTo, flightRegime);
					res.Add(delta);
				}

			//
			res.Days = GetDays(baseComponent.ManufactureDate.Value, effectiveDate); // +1 т.к. вторая граница интервала включительно

			// Сохраняем результат
			baseComponent.LifelengthCalculated.SetClosingLifelength(effectiveDate, flightRegime, res);
			//
			return new Lifelength(res);
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
		private async Task<Lifelength> getFlightLifelengthByPeriod(List<AircraftFlight> flights, BaseComponent bd, DateTime dateFrom, DateTime dateTo)
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
		private async Task<Lifelength> getFlightLifelength(AircraftFlight flight, BaseComponent bd)
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
