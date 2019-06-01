using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public interface IAircraftFlightRepository
	{
		Task<List<AircraftFlight>> GetAircraftFlightsByAircraftIdAsync(int aircraftId);
		Task<AircraftFlight> GetAircraftFlightsByIdAsync(int flightId);
	}
}