using System.Threading.Tasks;
using BusinessLayer.CalcView;
using BusinessLayer.Views;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public class AverageUtilizationRepository : IAverageUtilizationRepository
	{
		private readonly IAircraftRepository _aircraftRepository;
		private readonly IComponentRepository _componentRepository;

		public AverageUtilizationRepository(IAircraftRepository aircraftRepository, IComponentRepository componentRepository)
		{
			_aircraftRepository = aircraftRepository;
			_componentRepository = componentRepository;
		}

		public async Task<AverageUtilization> GetAverageUtillization(IDirective source)
		{
			if (source == null) return null;

			if (source is BaseComponentView) return ((BaseComponentView)source).AverageUtilization;

			var a = await _aircraftRepository.GetParentAircraftAsync(source);
			if (a != null)
			{
				var aircraftFrame = await _componentRepository.GetBaseComponentByIdAsync(a.AircraftFrameId);
				return aircraftFrame.AverageUtilization;
			}

			var s = await _storeCore.GetParentStoreAsync(source);
			return s != null ? new AverageUtilization(0, 0) : null;

		}
	}
}