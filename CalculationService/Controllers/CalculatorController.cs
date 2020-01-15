using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
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
		[HttpPost("GetFlightLifelengthForPeriod")]
		public async Task<IActionResult> GetFlightLifelengthForPeriod(int aircraftId, DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodAsync(aircraftId, dateFrom, dateTo);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnStartOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime currentDate)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayAsync(aircraftId, currentDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnEndOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime currentDate)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDayAsync(aircraftId, currentDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthIncludingThisFlight")]
		public async Task<IActionResult> GetFlightLifelengthIncludingThisFlight(int flightId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthIncludingThisFlightAsync(flightId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetCurrentFlightLifelength")]
		public async Task<IActionResult> GetCurrentFlightLifelength(int aircraftId)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelengthAsync(aircraftId);
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
