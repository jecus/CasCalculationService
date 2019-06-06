using System.Threading.Tasks;
using BusinessLayer.CalcView;
using BusinessLayer.Views;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public interface IMTOPCalculator
	{
		Task CalculateDirective(MaintenanceDirectiveView directive, AverageUtilization averageUtilization);
	}
}