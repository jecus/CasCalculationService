using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Views;
using CalculationService.Workers.Infrastructure;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalculationService.Workers
{
	public class AircraftFlightWorker : IWorker
	{
		private readonly ILogger<AircraftFlightWorker> _logger;
		private readonly DatabaseContext _db;

		public AircraftFlightWorker(ILogger<AircraftFlightWorker> logger, DatabaseContext db)
		{
			_logger = logger;
			_db = db;
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		public async void Start()
		{
			while (true)
			{
				try
				{
					GlobalObjects.Flights.Clear();
					var aircraftIds = await _db.Aircrafts.Select(i => i.Id).ToListAsync();

					Parallel.ForEach(aircraftIds.OrderBy(i => i), async id =>
					{
						var flights = await _db.AircraftFlights
							.Include(i => i.CancelReason)
							.Where(i => i.AircraftId == id)
							.OnlyActive()
							.AsNoTracking()
							.ToListAsync();

						GlobalObjects.Flights.Add(id, flights.Select(i => new AircraftFlightView(i)).ToList());
					});

					
					Thread.Sleep(TimeSpan.FromMinutes(5));
				}
				catch (Exception e)
				{
					_logger.LogDebug(e.Message);
				}
			}
		}

		#endregion
	}
}