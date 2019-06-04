using System.Threading.Tasks;
using BusinessLayer.Views;

namespace BusinessLayer.Repositiries
{
	public interface IAircraftRepository
	{
		Task<AircraftView> GetAircraftByIdAsync(int aircraftId);
	}
}