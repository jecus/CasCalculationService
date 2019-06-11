using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Views;
using CalculationService;
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

		public async Task<List<BaseComponentView>> GetBaseComponents()
		{
			var baseComponents = await _db.BaseComponents
				.Include(i => i.ActualStateRecords)
				.Include(i => i.ChangeLLPCategoryRecords)
				.Include(i => i.TransferRecords)
				.Include(i => i.Regimes)
				.Include(i => i.LLPData)
				.Include(i => i.ComponentDirectives)
				.AsNoTracking()
				.OnlyActive()
				.ToListAsync();

			if (baseComponents == null)
				return null;

			var res = baseComponents.Select(i => new BaseComponentView(i)).ToList();

			foreach (var baseComponent in res)
			{
				var last = baseComponent.TransferRecords.GetLast();
				if (last != null &&  last.DestinationObjectType == (int)SmartCoreType.Aircraft)
					baseComponent.AircaraftId = last.DestinationObjectId.Value;
			}
			
			return res;
		}

		public async Task<BaseComponentView> GetBaseComponentByIdAsync(int baseComponentId)
		{
			return GlobalObjects.BaseComponents.FirstOrDefault(i => i.Id == baseComponentId);
			//var baseComponent =  await _db.BaseComponents
			//	.Include(i => i.ActualStateRecords)
			//	.Include(i => i.ChangeLLPCategoryRecords)
			//	.Include(i => i.TransferRecords)
			//	.Include(i => i.Regimes)
			//	.Include(i => i.LLPData)
			//	.Include(i => i.ComponentDirectives)
			//	.AsNoTracking()
			//	.OnlyActive()
			//	.FirstOrDefaultAsync(i => i.Id == baseComponentId);

			//if (baseComponent == null)
			//	return null;

			//baseComponent.AircaraftId = await _db.GetDestinationObjectIdQueryFromQuery(GetQueryForParentAircraft(baseComponentId));
			//return new BaseComponentView(baseComponent);
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

		public async Task<ComponentDirectiveView> GetComponentDirectiveByIdAsync(int cdId)
		{
			try
			{
				var componentDirective = await _db.ComponentDirectives
					.Include(i => i.Component)
					.Include(i => i.Component.ActualStateRecords)
					.Include(i => i.Component.ChangeLLPCategoryRecords)
					.Include(i => i.Component.TransferRecords)
					.Include(i => i.Component.LLPData)
					.Include(i => i.PerformanceRecords)
					.AsNoTracking()
					.OnlyActive()
					.FirstOrDefaultAsync(i => i.Id == cdId);

				if (componentDirective == null)
					return null;

				return new ComponentDirectiveView(componentDirective);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
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

		public async Task<List<ComponentDirectiveView>> GetComponentDirectivessAsync(List<int> cdIds)
		{
			var directives = await _db.ComponentDirectives
				.Include(i => i.Component)
				.Include(i => i.Component.ActualStateRecords)
				.Include(i => i.Component.ChangeLLPCategoryRecords)
				.Include(i => i.Component.TransferRecords)
				.Include(i => i.Component.LLPData)
				.Include(i => i.PerformanceRecords)
				.AsNoTracking()
				.Where(i => cdIds.Contains(i.Id))
				.OnlyActive().ToListAsync();


			if (directives == null)
				return null;

			return directives.Select(i => new ComponentDirectiveView(i)).ToList();
		}


		private string GetQueryForParentAircraft(int baseComponentId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId 
                     from dbo.TransferRecords 
                      where IsDeleted=0 and ParentID = {baseComponentId} order by dbo.TransferRecords.TransferDate desc	";
		}
	}
}