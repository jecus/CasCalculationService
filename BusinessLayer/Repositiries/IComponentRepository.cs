using System.Threading.Tasks;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public interface IComponentRepository
	{
		Task<Component> GetBaseComponentByIdAsync(int baseComponentId);

	}
}