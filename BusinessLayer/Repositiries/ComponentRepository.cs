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
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == baseComponentId);

			baseComponent.AircaraftId = _db.GetDestinationObjectIdQueryFromQuery(GetQueryForParentAircraft(baseComponentId));
			return new BaseComponentView(baseComponent);
		}

		public async Task<ComponentView> GetComponentByIdAsync(int componentId)
		{
			var component = await _db.BaseComponents
				.Include(i => i.ActualStateRecords)
				.Include(i => i.ChangeLLPCategoryRecords)
				.Include(i => i.TransferRecords)
				.Include(i => i.Regimes)
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == componentId);

			return new ComponentView(component);
		}


		private string GetQueryForParentAircraft(int baseComponentId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId 
                     from dbo.TransferRecords 
                      where IsDeleted=0 and ParentID = {baseComponentId} order by dbo.TransferRecords.TransferDate desc	";
		}
	}
}