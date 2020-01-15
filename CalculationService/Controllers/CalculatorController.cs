using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
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
		[HttpPost("getFlightLifelengthForPeriod")]
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

		[HttpPost("getFlightLifelengthOnStartOfDay")]
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

		[HttpPost("getFlightLifelengthOnEndOfDay")]
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

		[HttpPost("getFlightLifelengthIncludingThisFlight")]
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

		[HttpPost("getCurrentFlightLifelength")]
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
	}
}
