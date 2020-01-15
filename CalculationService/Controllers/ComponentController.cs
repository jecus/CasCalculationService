using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
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
		[HttpPost("GetCurrentFlightLifelength")]
		public async Task<IActionResult> GetCurrentFlightLifelength(int componentId)
		{
			try
			{
				var res = await _calculator.GetCurrentFlightLifelength(componentId);
				return Ok(res);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest(new { Error = e.Message });
			}
		}

		[HttpPost("GetFlightLifelengthOnEndOfDay")]
		public async Task<IActionResult> GetFlightLifelengthOnEndOfDay(int componentId, DateTime effectiveDate)
		{
			try
			{
				var res = await _calculator.GetFlightLifelengthOnEndOfDay(componentId, effectiveDate);
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