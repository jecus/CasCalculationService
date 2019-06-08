using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.CalcView;
using BusinessLayer.Vendors;
using BusinessLayer.Views;
using Entity.Models;

namespace BusinessLayer.Calculator
{
	public interface ICalculator
	{

		Task<Lifelength> GetFlightLifelengthOnEndOfDay(BaseView directive, DateTime effectiveDate);
		Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseView directive, DateTime effectiveDate);

		/*
         * Расчет дат
         */

		DateTime GetMaxDate(DateTime dateTime1, DateTime dateTime2);
		DateTime GetManufactureDate(BaseView source);
		DateTime GetStartDate(IDirective directive);

		// Воздушное судно
		Task<Lifelength> GetFlightLifelengthOnStartOfDayAsync(int aircraftId, DateTime date);
		Task<Lifelength> GetFlightLifelengthOnEndOfDayAsync(int aircraftId, DateTime date);
		Task<Lifelength> GetFlightLifelengthForPeriodAsync(int aircraftId, DateTime fromDate, DateTime toDate);
		Task<Lifelength> GetFlightLifelengthIncludingThisFlightAsync(int flightId);
		Task<Lifelength> GetCurrentFlightLifelengthAsync(int aircraftId);

		// Базовый агрегат
		Task<Lifelength> GetFlightLifelengthIncludingThisFlightBaseComponentAsync(int baseComponentId, int flightId);
		Task<Lifelength> GetFlightLifelengthOnStartOfDayBaseComponentAsync(int baseComponentId, DateTime date);
		Task<Lifelength> GetFlightLifelengthOnStartOfDayBaseComponentAsync(int baseComponentId, DateTime date, int flightRegimeId);
		Task<Lifelength> GetCurrentFlightLifelengthBaseComponentAsync(int baseComponentId);
		Task<Lifelength> GetFlightLifelengthOnEndOfDayBaseComponentAsync(int baseComponentId, DateTime effectiveDate);
		Task<Lifelength> GetFlightLifelengthForPeriodBaseComponentAsync(int baseComponentId, DateTime fromDate, DateTime toDate);
		Task<Lifelength> GetFlightLifelengthForPeriodBaseComponentAsync(int baseComponentId, DateTime fromDate, DateTime toDate,
			int flightRegimeId);
		Task<Lifelength> GetFlightLifelengthBaseComponentAsync(int flightId, int baseComponentId);
		Task<Lifelength> GetFlightLifelengthBaseComponentAsync(int flightId, int baseComponentId, int flightRegimeId);

		// Агрегат
		Task<Lifelength> GetCurrentFlightLifelength(int componentId);
		Task<Lifelength> GetFlightLifelengthOnEndOfDay(int componentId, DateTime effectiveDate);
	}
}