using System.Threading.Tasks;
using BusinessLayer.CalcView;
using BusinessLayer.Views;

namespace BusinessLayer.Repositiries
{
	public interface IAircraftRepository
	{
		Task<AircraftView> GetAircraftByIdAsync(int aircraftId);
		Task<AircraftView> LoadAircraftByIdAsync(int aircraftId);
		Task<AircraftView> GetParentAircraftAsync(IDirective source);
	}
}