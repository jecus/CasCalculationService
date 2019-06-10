using System;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseUnitController : ControllerBase
    {
	    private readonly ICalculator _calculator;

	    public BaseUnitController(ICalculator calculator)
	    {
		    _calculator = calculator;
	    }

	    // GET api/values
	    [HttpPost("GetFlightLifelengthIncludingThisFlightBaseComponent")]
		public async Task<JsonResult> GetFlightLifelengthIncludingThisFlightBaseComponent(int baseComponentId, int flightId)
		{
		    var res = await _calculator.GetFlightLifelengthIncludingThisFlightBaseComponentAsync(baseComponentId, flightId);

			return new JsonResult(res);
	    }

		[HttpPost("GetFlightLifelengthOnStartOfDayBaseComponent")]
		public async Task<JsonResult> GetFlightLifelengthOnStartOfDayBaseComponent(int baseComponentId, DateTime date)
		{
			var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(baseComponentId, date);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnStartOfDayBaseComponentRegime")]
		public async Task<JsonResult> GetFlightLifelengthOnStartOfDayBaseComponent(int baseComponentId, DateTime date, int flightRegimeId)
		{
			var res = await _calculator.GetFlightLifelengthOnStartOfDayBaseComponentAsync(baseComponentId, date, flightRegimeId);

			return new JsonResult(res);
		}

		[HttpPost("GetCurrentFlightLifelengthBaseComponent")]
		public async Task<JsonResult> GetCurrentFlightLifelengthBaseComponent(int baseComponentId)
		{
			var res = await _calculator.GetCurrentFlightLifelengthBaseComponentAsync(baseComponentId);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnEndOfDayBaseComponent")]
		public async Task<JsonResult> GetFlightLifelengthOnEndOfDayBaseComponent(int baseComponentId, DateTime effectiveDate)
		{
			var res = await _calculator.GetFlightLifelengthOnEndOfDayBaseComponentAsync(baseComponentId, effectiveDate);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthForPeriodBaseComponent")]
		public async Task<JsonResult> GetFlightLifelengthForPeriodBaseComponent(int baseComponentId, DateTime fromDate, DateTime toDate)
		{
			var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(baseComponentId, fromDate, toDate);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthForPeriodBaseComponentWithRegime")]
		public async Task<JsonResult> GetFlightLifelengthForPeriodBaseComponent(int baseComponentId, DateTime fromDate, DateTime toDate, int flightRegimeId)
		{
			var res = await _calculator.GetFlightLifelengthForPeriodBaseComponentAsync(baseComponentId, fromDate, toDate, flightRegimeId);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthBaseComponent")]
		public async Task<JsonResult> GetFlightLifelengthBaseComponent(int flightId, int baseComponentId)
		{
			var res = await _calculator.GetFlightLifelengthBaseComponentAsync(flightId, baseComponentId);

			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthBaseComponentWithRegime")]
		public async Task<JsonResult> GetFlightLifelengthBaseComponent(int flightId, int baseComponentId, int flightRegimeId)
		{
			var res = await _calculator.GetFlightLifelengthBaseComponentAsync(flightId, baseComponentId, flightRegimeId);

			return new JsonResult(res);
		}
	}
}