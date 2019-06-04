using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Views;

namespace BusinessLayer.Repositiries
{
	public interface IAircraftFlightRepository
	{
		Task<List<AircraftFlightView>> GetAircraftFlightsByAircraftIdAsync(int aircraftId);
		Task<AircraftFlightView> GetAircraftFlightsByIdAsync(int flightId);
		Task<List<AircraftFlightView>> GetAircraftFlightsOnDate(int aircraftId, DateTime onDate);
	}
}