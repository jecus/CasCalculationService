using System;
using System.Linq;
using System.Threading;
using BusinessLayer.Calculator;
using BusinessLayer.Repositiries;
using CalculationService.Workers.Infrastructure;
using Entity;
using Microsoft.Extensions.Logging;

namespace CalculationService.Workers
{
	public class BaseComponentWorker : IWorker
	{
		private readonly ILogger<AircraftFlightWorker> _logger;
		private readonly IComponentRepository _componentRepository;
		private readonly ICalculator _calculator;
		private readonly DatabaseContext _db;

		public BaseComponentWorker(ILogger<AircraftFlightWorker> logger, IComponentRepository componentRepository,
			ICalculator calculator)
		{
			_logger = logger;
			_componentRepository = componentRepository;
			_calculator = calculator;
		}

		#region Implementation of IDisposable

		public void Dispose()
		{

		}

		public async void Start()
		{
			_logger.LogInformation($"BaseComponent Worker started!");


			_logger.LogInformation($"Load BaseComponent({DateTime.Now})");
			try
			{
				Thread.Sleep(TimeSpan.FromMinutes(1));

				var res = await _componentRepository.GetBaseComponents();
				GlobalObjects.BaseComponents.Clear();

				var aircraftIds = GlobalObjects.Flights.Select(i => i.Key);
				foreach (var bc in res)
				{
					var tr = bc.TransferRecords.FirstOrDefault(i =>
						i.DestinationObjectType == (int) SmartCoreType.Aircraft);
					if (tr != null && aircraftIds.Contains(tr.DestinationObjectId.Value))
						GlobalObjects.BaseComponents.Add(bc);
				}


				while (true)
				{
					foreach (var baseComponent in GlobalObjects.BaseComponents)
					{
						baseComponent.LifelengthCalculated.Clear();
						
						await _calculator.GetFlightLifelengthOnEndOfDayBaseComponentAsync(baseComponent.Id,
							DateTime.Today);
					}
				}

				Thread.Sleep(TimeSpan.FromMinutes(30));
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			#endregion
		}
	}
}
