﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private readonly ICalculator _calculator;

		public CalculatorController(ICalculator calculator)
		{
			_calculator = calculator;
		}

		// GET api/values
		[HttpPost("GetFlightLifelengthForPeriod")]
		public async Task<JsonResult> GetFlightLifelengthForPeriod(int aircraftId, DateTime dateFrom, DateTime dateTo)
		{
			var res = await _calculator.GetFlightLifelengthForPeriod(aircraftId, dateFrom, dateTo);
			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnStartOfDay")]
		public async Task<JsonResult> GetFlightLifelengthOnStartOfDay(int aircraftId, DateTime currentDate)
		{
			var res = await _calculator.GetFlightLifelengthOnStartOfDay(aircraftId, currentDate);
			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthOnEndOfDay")]
		public async Task<JsonResult> GetFlightLifelengthOnEndOfDay(int aircraftId, DateTime currentDate)
		{
			var res = await _calculator.GetFlightLifelengthOnEndOfDay(aircraftId, currentDate);
			return new JsonResult(res);
		}

		[HttpPost("GetFlightLifelengthIncludingThisFlight")]
		public async Task<JsonResult> GetFlightLifelengthIncludingThisFlight(int flightId)
		{
			var res = await _calculator.GetFlightLifelengthIncludingThisFlight(flightId);
			return new JsonResult(res);
		}

		[HttpPost("GetCurrentFlightLifelength")]
		public async Task<JsonResult> GetCurrentFlightLifelength(int aircraftId)
		{
			var res = await _calculator.GetCurrentFlightLifelength(aircraftId);
			return new JsonResult(res);
		}
	}
}