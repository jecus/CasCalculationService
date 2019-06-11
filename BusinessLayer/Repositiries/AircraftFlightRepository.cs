using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Views;
using CalculationService;
using Entity;

namespace BusinessLayer.Repositiries
{
	public class AircraftFlightRepository : IAircraftFlightRepository
	{
		private readonly DatabaseContext _db;

		public AircraftFlightRepository(DatabaseContext db)
		{
			_db = db;
		}

		public async Task<List<AircraftFlightView>> GetAircraftFlightsByAircraftIdAsync(int aircraftId)
		{
			GlobalObjects.Flights.TryGetValue(aircraftId, out var flights);
			return flights;

			//var atlbs = await _db.Atlbs
			//	.Where(i => i.AircraftID == aircraftId)
			//	.OnlyActive()
			//	.AsNoTracking()
			//	.ToListAsync();
			//var ids = atlbs.Select(i => i.Id).ToList();

			//var res = await _db.AircraftFlights
			//	.Include(i => i.CancelReason)
			//	.AsNoTracking()
			//	.OnlyActive()
			//	.Where(i => ids.Contains(i.ATLBID))
			//	.ToListAsync();

			//if (res == null)
			//	return null;

			//return res.Select(i => new AircraftFlightView(i)).ToList();
		}

		public async Task<AircraftFlightView> GetAircraftFlightsByIdAsync(int flightId)
		{
			return GlobalObjects.Flights.SelectMany(i => i.Value).FirstOrDefault(i => i.Id == flightId);
			//var res = await _db.AircraftFlights
			//	.Include(i => i.CancelReason)
			//	.AsNoTracking()
			//	.OnlyActive()
			//	.FirstOrDefaultAsync(i => i.Id == flightId);

			//if (res == null)
			//	return null;

			//return new AircraftFlightView(res);
		}

		public async Task<List<AircraftFlightView>> GetAircraftFlightsOnDate(int aircraftId, DateTime onDate)
		{
			GlobalObjects.Flights.TryGetValue(aircraftId, out var flights);
			return flights.Where(i => i.AircraftId == aircraftId && i.FlightDate <= onDate).ToList();

			//var res = await _db.AircraftFlights
			//	.Include(i => i.CancelReason)
			//	.AsNoTracking()
			//	.OnlyActive()
			//	.Where(i => i.AircraftId == aircraftId && i.FlightDate <= onDate)
			//	.ToListAsync();

			//if (res == null)
			//	return null;

			//return res.Select(i => new AircraftFlightView(i)).ToList();
		}
	}
}