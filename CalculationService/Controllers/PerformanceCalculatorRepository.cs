using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PerformanceCalculatorRepository : ControllerBase
	{
		private readonly IPerformanceCalculator _performanceCalculator;

		public PerformanceCalculatorRepository(IPerformanceCalculator performanceCalculator)
		{
			_performanceCalculator = performanceCalculator;
		}

		[HttpPost("NextPerformanceForComponent")]
		public async Task<JsonResult> NextPerformanceForComponent(int componentId)
		{
			var res = await _performanceCalculator.NextPerformanceForComponent(componentId);
			return new JsonResult(res);
		}

		[HttpPost("NextPerformanceForComponents")]
		public async Task<JsonResult> NextPerformanceForComponents(List<int> componentIds)
		{
			var res = await _performanceCalculator.NextPerformanceForComponents(componentIds);
			return new JsonResult(res);
		}

		[HttpPost("NextPerformanceForComponentDirective")]
		public async Task<JsonResult> NextPerformanceForComponentDirective(int componentDirectiveId)
		{
			var res = await _performanceCalculator.NextPerformanceForComponentDirective(componentDirectiveId);
			return new JsonResult(res);
		}

		[HttpPost("NextPerformanceForComponentDirectives")]
		public async Task<JsonResult> NextPerformanceForComponentDirectives(List<int> componentDirectiveIds)
		{
			var res = await _performanceCalculator.NextPerformanceForComponentDirectives(componentDirectiveIds);
			return new JsonResult(res);
		}
	}
}