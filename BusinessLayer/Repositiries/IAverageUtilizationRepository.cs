using System.Threading.Tasks;
using BusinessLayer.CalcView;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public interface IAverageUtilizationRepository
	{
		Task<AverageUtilization> GetAverageUtillization(IDirective source);
	}
}