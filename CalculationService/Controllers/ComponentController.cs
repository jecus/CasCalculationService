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
	public class ComponentController : ControllerBase
	{
		private readonly ICalculator _calculator;
		private readonly ILogger<CalculatorController> _logger;

		public ComponentController(ICalculator calculator, ILogger<CalculatorController> logger)
		{
			_calculator = calculator;
			_logger = logger;
		}

		// GET api/values
		[HttpPost("getCurrentFlightLifelength")]
		public async Task<IActionResult> GetCurrentFlightLifelength(InComponentView view)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelength(view.ComponentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("getFlightLifelengthOnEndOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDay(InComponentView view)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDay(view.ComponentId, view.EffectiveDate);
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