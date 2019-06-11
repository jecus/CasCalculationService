using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ComponentController : ControllerBase
    {
		private readonly ICalculator _calculator;

		public ComponentController(ICalculator calculator)
		{
			_calculator = calculator;
		}

		// GET api/values
		[HttpPost("GetCurrentFlightLifelength")]
		public async Task<JsonResult> GetCurrentFlightLifelength(int componentId)
		{
			var res = await _calculator.GetCurrentFlightLifelength(componentId);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnEndOfDay")]
		public async Task<JsonResult> GetFlightLifelengthOnEndOfDay(int componentId, DateTime effectiveDate)
		{
			var res = await _calculator.GetFlightLifelengthOnEndOfDay(componentId, effectiveDate);

			return new JsonResult(res);
		}

	}
}