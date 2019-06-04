using System.Threading.Tasks;
using BusinessLayer.Calculator;
using BusinessLayer.Views;
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
			var frameId =  _db.GetIdFromQuery(GetQueryFrameId(aircraftId));
			var aircraft = await _db.Aircrafts
				.AsNoTracking()
				.OnlyActive()
				.FirstOrDefaultAsync(i => i.Id == aircraftId);
			aircraft.AircraftFrameId = frameId;
			return new AircraftView(aircraft);
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
