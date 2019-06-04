﻿using System.Threading.Tasks;
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
				.AsNoTracking()
				.OnlyActive()
				.Include(i => i.ActualStateRecords)
				.FirstOrDefaultAsync(i => i.Id == baseComponentId);

			baseComponent.AircaraftId = _db.GetDestinationObjectIdQueryFromQuery(GetQueryForParentAircraft(baseComponentId));
			return new BaseComponentView(baseComponent);
		}


		private string GetQueryForParentAircraft(int baseComponentId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId 
                     from dbo.TransferRecords 
                      where IsDeleted=0 and ParentID = {baseComponentId} order by dbo.TransferRecords.TransferDate desc	";
		}
	}
}