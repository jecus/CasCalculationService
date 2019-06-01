using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Entity;
using Entity.Entity;
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

		public async Task<List<AircraftFlight>> GetAircraftFlightsByAircraftIdAsync(int aircraftId)
		{
			var atlbs = await _db.Atlbs
				.Where(i => i.AircraftID == aircraftId)
				.OnlyActive()
				.AsNoTracking()
				.ToListAsync();
			var ids = atlbs.Select(i => i.Id).ToList();

			return await _db.AircraftFlights
				.AsNoTracking()
				.OnlyActive()
				.Where(i => ids.Contains(i.ATLBID))
				.ToListAsync();
		}

		public async Task<AircraftFlight> GetAircraftFlightsByIdAsync(int flightId)
		{
			return await _db.AircraftFlights
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == flightId);
		}
	}
}