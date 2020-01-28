using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Vendors;
using BusinessLayer.Views.In;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private readonly ICalculator _calculator;
		private readonly ILogger<CalculatorController> _logger;

		public CalculatorController(ICalculator calculator, ILogger<CalculatorController> logger)
		{
			_calculator = calculator;
			_logger = logger;
		}

		// GET api/values
		[HttpPost("forperiod")]
		public async Task<IActionResult> GetFlightLifelengthForPeriod(InCalculatorView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodAsync(view.AircraftId, view.DateFrom, view.DateTo);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("onstartofday")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDay(InCalculatorView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayAsync(view.AircraftId, view.CurrentDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("onendofday")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDay(InCalculatorView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDayAsync(view.AircraftId, view.CurrentDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("includingthisflight")]
		public async Task<IActionResult> GetFlightLifelengthIncludingThisFlight(InCalculatorView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthIncludingThisFlightAsync(view.FlightId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("currentflight")]
		public async Task<IActionResult> GetCurrentFlightLifelength(InCalculatorView view)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelengthAsync(view.AircraftId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("resethmath")]
		public async Task<IActionResult> ResethMath(InCalculatorView view)
		{
			try
			{
				var baseComponents = GlobalObjects.BaseComponents.Where(i => i.AircaraftId == view.AircraftId);

				foreach (var baseComponent in baseComponents)
				{
					if (baseComponent.LifelengthCalculated != null)
						baseComponent.LifelengthCalculated.Clear();
					else baseComponent.LifelengthCalculated = new LifelengthCollection(baseComponent.ManufactureDate);
				}

				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}


		}
	}
}
