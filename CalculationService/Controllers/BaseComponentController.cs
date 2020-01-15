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
		[HttpPost("getFlightLifelengthIncludingThisFlight")]
		public async Task<IActionResult> GetFlightLifelengthIncludingThisFlightBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthIncludingThisFlightBaseComponentAsync(view.BaseComponentId, view.FlightId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthOnStartOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDayBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(view.BaseComponentId, view.Date);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthOnStartOfDayRegime")]
		public async Task<IActionResult> GetFlightLifelengthOnStartOfDayRegimeBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(view.BaseComponentId, view.Date, view.FlightRegimeId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getCurrentFlightLifelength")]
		public async Task<IActionResult> GetCurrentFlightLifelengthBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelengthBaseComponentAsync(view.BaseComponentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthOnEndOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDayBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDayBaseComponentAsync(view.BaseComponentId, view.EffectiveDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthForPeriod")]
		public async Task<IActionResult> GetFlightLifelengthForPeriodBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(view.BaseComponentId, view.FromDate, view.ToDate);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthForPeriodWithRegime")]
		public async Task<IActionResult> GetFlightLifelengthForPeriodWithRegimeBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(view.BaseComponentId, view.FromDate, view.ToDate, view.FlightRegimeId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelength")]
		public async Task<IActionResult> GetFlightLifelengthBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthBaseComponentAsync(view.FlightId, view.BaseComponentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthWithRegime")]
		public async Task<IActionResult> GetFlightLifelengthWithRegimeBaseComponent(InBaseComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthBaseComponentAsync(view.FlightId, view.BaseComponentId, view.FlightRegimeId);
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