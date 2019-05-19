using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly ICalculator _calculator;

		public ValuesController(ICalculator calculator)
		{
			_calculator = calculator;
		}

		// GET api/values
		[HttpGet]
		public async Task<JsonResult> Get()
		{
			var res = await _calculator.GetFlightLifelengthOnStartOfDay(2348, DateTime.Today);
			return new JsonResult(res);
		}
	}
}
