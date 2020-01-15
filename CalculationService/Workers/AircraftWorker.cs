using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Repositiries;
using BusinessLayer.Views;
using CalculationService.Workers.Infrastructure;
using Entity;
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalculationService.Workers
{
	public class AircraftWorker : IWorker
	{
		private readonly ILogger<AircraftWorker> _logger;
		private readonly DatabaseContext _db;
		private readonly IAircraftRepository _aircraftRepository;

		public AircraftWorker(ILogger<AircraftWorker> logger, DatabaseContext db, IAircraftRepository aircraftRepository)
		{
			_logger = logger;
			_db = db;
			_aircraftRepository = aircraftRepository;
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
				_logger.LogInformation($"Load Aircrafts({DateTime.Now})");
				try
				{
					var temp = new List<AircraftView>();
					
					var aircraftIds = await _db.Aircrafts.OnlyActive().Select(i => i.Id).ToListAsync();
					foreach (var id in aircraftIds)
						temp.Add(await _aircraftRepository.LoadAircraftByIdAsync(id));

					GlobalObjects.Aircrafts.Clear();
					GlobalObjects.Aircrafts.AddRange(temp);
					Thread.Sleep(TimeSpan.FromDays(1));
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