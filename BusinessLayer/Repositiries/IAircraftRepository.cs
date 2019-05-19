using System.Threading.Tasks;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public interface IAircraftRepository
	{
		Task<Aircraft> GetAircraftByIdAsync(int aircraftId);
	}
}