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
		public async Task<JsonResult> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime currentDate)
		{
			var res = await _calculator.GetFlightLifelengthOnStartOfDayAsync(aircraftId, currentDate);
			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnEndOfDay")]
		public async Task<JsonResult> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime currentDate)
		{
			var res = await _calculator.GetFlightLifelengthOnEndOfDayAsync(aircraftId, currentDate);
			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthIncludingThisFlight")]
		public async Task<JsonResult> GetFlightLifelengthIncludingThisFlight(int flightId)
		{
			var res = await _calculator.GetFlightLifelengthIncludingThisFlightAsync(flightId);
			return new JsonResult(res);
		}

		[HttpPost("GetCurrentFlightLifelength")]
		public async Task<JsonResult> GetCurrentFlightLifelength(int aircraftId)
		{
			var res = await _calculator.GetCurrentFlightLifelengthAsync(aircraftId);
			return new JsonResult(res);
		}
	}
}
