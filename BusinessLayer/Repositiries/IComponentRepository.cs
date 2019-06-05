using System.Threading.Tasks;
using BusinessLayer.Views;

namespace BusinessLayer.Repositiries
{
	public interface IComponentRepository
	{
		Task<BaseComponentView> GetBaseComponentByIdAsync(int baseComponentId);
		Task<ComponentView> GetComponentByIdAsync(int componentId);

	}
}