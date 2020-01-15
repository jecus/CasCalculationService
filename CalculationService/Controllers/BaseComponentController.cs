using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseComponentController : ControllerBase
	{
		private readonly ICalculator _calculator;
		private readonly ILogger<CalculatorController> _logger;

		public BaseComponentController(ICalculator calculator, ILogger<CalculatorController> logger)
		{
			_calculator = calculator;
			_logger = logger;
		}

		// GET api/values
		[HttpPost("GetFlightLifelengthIncludingThisFlightBaseComponent")]
		public async Task<IActionResult> GetFlightLifelengthIncludingThisFlightBaseComponent(int baseComponentId, int flightId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthIncludingThisFlightBaseComponentAsync(baseComponentId, flightId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnStartOfDayBaseComponent")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDayBaseComponent(int baseComponentId, DateTime date)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(baseComponentId, date);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnStartOfDayBaseComponentRegime")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDayBaseComponent(int baseComponentId, DateTime date, int flightRegimeId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(baseComponentId, date, flightRegimeId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetCurrentFlightLifelengthBaseComponent")]
		public async Task<IActionResult> GetCurrentFlightLifelengthBaseComponent(int baseComponentId)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelengthBaseComponentAsync(baseComponentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnEndOfDayBaseComponent")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDayBaseComponent(int baseComponentId, DateTime effectiveDate)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDayBaseComponentAsync(baseComponentId, effectiveDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthForPeriodBaseComponent")]
		public async Task<IActionResult> GetFlightLifelengthForPeriodBaseComponent(int baseComponentId, DateTime fromDate, DateTime toDate)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(baseComponentId, fromDate, toDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthForPeriodBaseComponentWithRegime")]
		public async Task<IActionResult> GetFlightLifelengthForPeriodBaseComponent(int baseComponentId, DateTime fromDate, DateTime toDate, int flightRegimeId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(baseComponentId, fromDate, toDate, flightRegimeId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthBaseComponent")]
		public async Task<IActionResult> GetFlightLifelengthBaseComponent(int flightId, int baseComponentId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthBaseComponentAsync(flightId, baseComponentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthBaseComponentWithRegime")]
		public async Task<IActionResult> GetFlightLifelengthBaseComponent(int flightId, int baseComponentId, int flightRegimeId)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthBaseComponentAsync(flightId, baseComponentId, flightRegimeId);
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