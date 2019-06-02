using System.Threading.Tasks;
using Entity.Entity;

namespace BusinessLayer.Repositiries
{
	public interface IComponentRepository
	{
		Task<BaseComponent> GetBaseComponentByIdAsync(int baseComponentId);

	}
}