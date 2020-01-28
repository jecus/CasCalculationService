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
		[HttpPost("includingthisflight")]
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

		[HttpPost("onstartofday")]
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

		[HttpPost("onstartofdayregime")]
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

		[HttpPost("currentflight")]
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

		[HttpPost("onendofday")]
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

		[HttpPost("forperiod")]
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

		[HttpPost("forperiodwithregime")]
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

		[HttpPost("flight")]
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

		[HttpPost("withregime")]
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

		[HttpPost("resethmath")]
		public async Task<IActionResult> ResethMath(InBaseComponentView view)
		{
			try
			{
				var baseComponent = GlobalObjects.BaseComponents.FirstOrDefault(i => i.Id == view.BaseComponentId);

				if (baseComponent != null)
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