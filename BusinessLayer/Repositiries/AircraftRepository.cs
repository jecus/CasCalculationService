using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.CalcView;
using BusinessLayer.Views;
using CalculationService;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositiries
{
	public class AircraftRepository : IAircraftRepository
	{
		private readonly DatabaseContext _db;

		public AircraftRepository(DatabaseContext db)
		{
			_db = db;
		}

		public async Task<AircraftView> GetAircraftByIdAsync(int aircraftId)
		{
			return GlobalObjects.Aircrafts.FirstOrDefault(i => i.Id == aircraftId);
		}

		public async Task<AircraftView> LoadAircraftByIdAsync(int aircraftId)
		{
			var frameId = await _db.GetIdFromQuery(GetQueryFrameId(aircraftId));
			var aircraft = await _db.Aircrafts
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == aircraftId);

			if (aircraft == null)
				return null;

			aircraft.AircraftFrameId = frameId;
			return new AircraftView(aircraft);
		}

		public async Task<AircraftView> GetParentAircraftAsync(IDirective source)
		{
			if (source is MaintenanceDirectiveView)
			{
				var aircraftId =await _db.GetDestinationObjectIdQueryFromQuery(GetQueryFrameId(((MaintenanceDirectiveView)source).Id));
				return await GetAircraftByIdAsync(aircraftId);
			}
			if (source is ComponentView)
			{
				var baseComponentId = await _db.GetDestinationObjectIdQueryFromQuery(GetQueryComponentAircraft(((ComponentView)source).Id));
				var aircraftId = await _db.GetDestinationObjectIdQueryFromQuery(GetQueryComponentAircraft(baseComponentId));
				return await GetAircraftByIdAsync(aircraftId);
			}
			if(source is ComponentDirectiveView)
			{
				var aircraftId = await _db.GetDestinationObjectIdQueryFromQuery(GetQueryComponentDirectiveAircraft(((ComponentDirectiveView)source).Id));
				return await GetAircraftByIdAsync(aircraftId);
			}
			return null;
		}

		private string GetQueryMpdAircraft(int mpdId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId from dbo.TransferRecords where IsDeleted=0 
					and ParentID = (select mpd.ComponentId from MaintenanceDirectives mpd where mpd.ItemId = {mpdId} )
						order by dbo.TransferRecords.TransferDate desc";
		}

		private string GetQueryComponentAircraft(int componentId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId 
                     from dbo.TransferRecords 
                      where IsDeleted=0 and ParentID = {componentId} order by dbo.TransferRecords.TransferDate desc	";
		}



		private string GetQueryComponentDirectiveAircraft(int cdId)
		{
			return $@"select top 1 TransferRecords.DestinationObjectId from dbo.TransferRecords where IsDeleted=0 
					and ParentID = (select top 1 TransferRecords.DestinationObjectId from dbo.TransferRecords where IsDeleted=0 
					and ParentID = (select ComponentId from ComponentDirectives where ItemID = {cdId})
						order by dbo.TransferRecords.TransferDate desc)
						order by dbo.TransferRecords.TransferDate desc";
		}

		private string GetQueryFrameId(int aircraftId)
		{
			return $@"Select top 1 ItemID from dbo.Components
                                                 where IsBaseComponent = 1 and BaseComponentTypeId=4 and IsDeleted=0 and
	                                             (Select top 1 DestinationObjectId from dbo.TransferRecords Where 
					                              ParentType = 6 
                                                  and DestinationObjectType = 7
					                              and ParentId = dbo.Components.ItemId 
					                              and IsDeleted = 0
					                              order by dbo.TransferRecords.TransferDate Desc) = {aircraftId}";
		}
	}
}
