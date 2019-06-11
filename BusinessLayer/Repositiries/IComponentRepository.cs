using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Views;

namespace BusinessLayer.Repositiries
{
	public interface IComponentRepository
	{
		Task<List<BaseComponentView>> GetBaseComponents();
		Task<BaseComponentView> GetBaseComponentByIdAsync(int baseComponentId);

		Task<ComponentView> GetComponentByIdAsync(int componentId);
		Task<ComponentDirectiveView> GetComponentDirectiveByIdAsync(int cdId);

		Task<List<ComponentView>> GetComponentsAsync(List<int> componentIds);
		Task<List<ComponentDirectiveView>> GetComponentDirectivessAsync(List<int> cdIds);

	}
}