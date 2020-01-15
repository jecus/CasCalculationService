using System;
using System.Collections.Generic;
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
			_logger.LogInformation($"Flight Worker started!");

			while (true)
			{
				_logger.LogInformation($"Load Flights({DateTime.Now})");
				try
				{
					
					var aircraftIds = await _db.Aircrafts.OnlyActive().Select(i => i.Id).ToListAsync();
					var atlbs = await _db.Atlbs
						.Where(i => aircraftIds.Contains(i.AircraftID.Value))
						.OnlyActive()
						.AsNoTracking()
						.ToListAsync();

					var temp = new Dictionary<int, List<AircraftFlightView>>();

					//Parallel.ForEach(aircraftIds.OrderBy(i => i), async id =>
					//{
					//	var ids = atlbs.Where(i => i.AircraftID == id).Select(i => i.Id).ToList();
					//	var flights = await _db.AircraftFlights
					//		.Include(i => i.CancelReason)
					//		.Where(i => ids.Contains(i.ATLBID))
					//		.OnlyActive()
					//		.AsNoTracking()
					//		.ToListAsync();

					//	temp.Add(id, flights.Select(i => new AircraftFlightView(i)).ToList());
					//});


					foreach (var id in aircraftIds.OrderBy(i => i))
					{
						var ids = atlbs.Where(i => i.AircraftID == id).Select(i => i.Id).ToList();
						var flights = await _db.AircraftFlights
							.Include(i => i.CancelReason)
							.Where(i => ids.Contains(i.ATLBID))
							.OnlyActive()
							.AsNoTracking()
							.ToListAsync();

						temp.Add(id, flights.Select(i => new AircraftFlightView(i)).ToList());
					}

					GlobalObjects.Flights.Clear();
					GlobalObjects.Flights = temp;
					Thread.Sleep(TimeSpan.FromMinutes(5));
				}
				catch (Exception e)
				{
					_logger.LogError(e.Message);
				}
			}
		}

		#endregion
	}
}