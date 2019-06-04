using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.Vendors;
using BusinessLayer.Views;
using Entity.Models;

namespace BusinessLayer.Calculator
{
	public interface ICalculator
	{
		Task<Lifelength> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime date);
		Task<Lifelength> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime date);
		Task<Lifelength> GetFlightLifelengthForPeriod(int aircraftId, DateTime fromDate, DateTime toDate);
		Task<Lifelength> GetFlightLifelengthIncludingThisFlight(int flightId);
		Task<Lifelength> GetCurrentFlightLifelength(int aircraftId);

		/*
	    * Базовый агрегат
	    */
		Task<Lifelength> GetFlightLifelengthIncludingThisFlight(BaseComponentView baseComponent, AircraftFlightView flight);
		Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseComponentView baseComponent, DateTime date);
		Task<Lifelength> GetFlightLifelengthOnStartOfDay(BaseComponentView baseComponent, DateTime date, FlightRegime flightRegime);
		Task<Lifelength> GetCurrentFlightLifelength(BaseComponentView baseComponent);
		Task<Lifelength> GetFlightLifelengthOnEndOfDay(BaseComponentView baseComponent, DateTime effectiveDate);
		Task<Lifelength> GetFlightLifelengthForPeriod(BaseComponentView baseComponent, DateTime fromDate, DateTime toDate);
		Task<Lifelength> GetFlightLifelengthForPeriod(BaseComponentView baseComponent, DateTime fromDate, DateTime toDate, FlightRegime flightRegime);
		Task<Lifelength> GetFlightLifelength(AircraftFlightView flight, BaseComponentView bd);
		Task<Lifelength> GetFlightLifelength(AircraftFlightView flight, BaseComponentView bd, FlightRegime flightRegime);
	}
}