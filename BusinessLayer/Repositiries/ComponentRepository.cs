using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Views;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositiries
{
	public class ComponentRepository : IComponentRepository
	{
		private readonly DatabaseContext _db;

		public ComponentRepository(DatabaseContext db)
		{
			_db = db;
		}
		public async Task<BaseComponentView> GetBaseComponentByIdAsync(int baseComponentId)
		{
			var baseComponent =  await _db.BaseComponents
				.Include(i => i.ActualStateRecords)
				.Include(i => i.ChangeLLPCategoryRecords)
				.Include(i => i.TransferRecords)
				.Include(i => i.Regimes)
				.Include(i => i.LLPData)
				.Include(i => i.ComponentDirectives)
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == baseComponentId);

			if (baseComponent == null)
				return null;

			baseComponent.AircaraftId = await _db.GetDestinationObjectIdQueryFromQuery(GetQueryForParentAircraft(baseComponentId));
			return new BaseComponentView(baseComponent);
		}

		public async Task<ComponentView> GetComponentByIdAsync(int componentId)
		{
			var component = await _db.Components
				.Include(i => i.ActualStateRecords)
				.Include(i => i.ChangeLLPCategoryRecords)
				.Include(i => i.TransferRecords)
				.Include(i => i.LLPData)
				.Include(i => i.ComponentDirectives)
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == componentId);

			if (component == null)
				return null;

			return new ComponentView(component);
		}

		public async Task<List<ComponentView>> GetComponentsAsync(List<int> componentIds)
		{
			var components = await _db.Components
				.Include(i => i.ActualStateRecords)
				.Include(i => i.ChangeLLPCategoryRecords)
				.Include(i => i.TransferRecords)
				.Include(i => i.LLPData)
				.Include(i => i.ComponentDirectives)
				.AsNoTracking()
				.Where(i => componentIds.Contains(i.Id))
				.OnlyActive().ToListAsync();
				

			if (components == null)
				return null;

			return components.Select(i => new ComponentView(i)).ToList();  
		}


		private string GetQueryForParentAircraft(int baseComponentId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId 
                     from dbo.TransferRecords 
                      where IsDeleted=0 and ParentID = {baseComponentId} order by dbo.TransferRecords.TransferDate desc	";
		}
	}
}