using System;
using System.Linq;
using System.Threading;
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
					var flights = await _db.AircraftFlights
						.Include(i => i.CancelReason)
						.OnlyActive()
						.ToListAsync();

					GlobalObjects.Flights.Clear();
					foreach (var fl in flights.Where(i => i.AircraftId != null).GroupBy(i => i.AircraftId))
						GlobalObjects.Flights.Add(fl.Key.Value, fl.Select(i => new AircraftFlightView(i)).ToList());

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