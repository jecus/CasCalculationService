using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Vendors;

namespace BusinessLayer.Calculator
{
	public interface IPerformanceCalculator
	{
		Task<List<NextPerformance>> NextPerformanceForComponent(int componentId);
		Task<Dictionary<int, List<NextPerformance>>> NextPerformanceForComponents(List<int> componentIds);
	}
}