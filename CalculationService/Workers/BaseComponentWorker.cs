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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalculationService.Workers
{
	public class BaseComponentWorker : IWorker
	{
		private readonly ILogger<AircraftFlightWorker> _logger;
		private readonly IComponentRepository _componentRepository;
		private readonly DatabaseContext _db;

		public BaseComponentWorker(ILogger<AircraftFlightWorker> logger, IComponentRepository componentRepository)
		{
			_logger = logger;
			_componentRepository = componentRepository;
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		public async void Start()
		{
			_logger.LogInformation($"BaseComponent Worker started!");

			while (true)
			{
				_logger.LogInformation($"Load BaseComponent({DateTime.Now})");
				try
				{
					var res = await _componentRepository.GetBaseComponents();
					GlobalObjects.BaseComponents.Clear();
					GlobalObjects.BaseComponents.AddRange(res);
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