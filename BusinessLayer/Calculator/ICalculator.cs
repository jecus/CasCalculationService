using System;
using System.Threading.Tasks;
using Entity.Entity;
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
	}
}