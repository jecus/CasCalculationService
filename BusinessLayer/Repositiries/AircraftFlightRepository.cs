using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Views;
using Entity;
using Microsoft.EntityFrameworkCore;

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
			var atlbs = await _db.Atlbs
				.Where(i => i.AircraftID == aircraftId)
				.OnlyActive()
				.AsNoTracking()
				.ToListAsync();
			var ids = atlbs.Select(i => i.Id).ToList();

			var res = await _db.AircraftFlights
				.AsNoTracking()
				.OnlyActive()
				.Where(i => ids.Contains(i.ATLBID))
				.ToListAsync();

			return res.Select(i => new AircraftFlightView(i)).ToList();
		}

		public async Task<AircraftFlightView> GetAircraftFlightsByIdAsync(int flightId)
		{
			var res = await _db.AircraftFlights
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == flightId);

			return new AircraftFlightView(res);
		}

		public async Task<List<AircraftFlightView>> GetAircraftFlightsOnDate(int aircraftId, DateTime onDate)
		{
			var res = await _db.AircraftFlights
				.AsNoTracking()
				.OnlyActive()
				.Where(i => i.AircraftId == aircraftId && i.FlightDate <= onDate)
				.ToListAsync();

			return res.Select(i => new AircraftFlightView(i)).ToList();
		}
	}
}