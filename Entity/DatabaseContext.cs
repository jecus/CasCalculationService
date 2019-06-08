using System.Linq;
using System.Threading.Tasks;
using Entity.Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
	public class DatabaseContext : DbContext
	{
		public DbSet<AircraftFlight> AircraftFlights { get; set; }
		public DbSet<DirectiveRecord> DirectiveRecords { get; set; }
		public DbSet<EngineTimeInRegime> EngineTimeInRegimes { get; set; }
		public DbSet<MaintenanceDirective> MaintenanceDirectives { get; set; }
		public DbSet<ATLB> Atlbs { get; set; }
		public DbSet<Aircraft> Aircrafts { get; set; }
		public DbSet<Component> Components { get; set; }
		public DbSet<ComponentDirective> ComponentDirectives { get; set; }
		public DbSet<BaseComponent> BaseComponents { get; set; }
		public DbSet<ActualStateRecord> ActualStateRecords { get; set; }
		public DbSet<Reason> Reason { get; set; }
		public DbSet<TransferRecord> Set { get; set; }
		public DbSet<ComponentLLPCategoryChangeRecord> ComponentLLPCategoryChangeRecords { get; set; }
		private DbSet<IdQuery> IdQuery { get; set; }
		private DbSet<DestinationObjectIdQuery> DestinationObjectIdQuery { get; set; }

		#region  public DatabaseContext(DbContextOptions<DatabaseContext> opt)

		public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
		{
			Database.SetCommandTimeout(610);
		}

		#endregion

		#region protected override void OnModelCreating(ModelBuilder modelBuilder)

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Component>()
				.HasMany(i => i.TransferRecords).WithOne(i => i.Component).HasForeignKey(i => i.ParentID);
			modelBuilder.Entity<Component>()
				.HasMany(i => i.ActualStateRecords).WithOne(i => i.Component).HasForeignKey(i => i.ComponentId);
			modelBuilder.Entity<Component>()
				.HasMany(i => i.ChangeLLPCategoryRecords).WithOne(i => i.Component).HasForeignKey(i => i.ParentId);
			modelBuilder.Entity<Component>()
				.HasMany(i => i.ComponentDirectives).WithOne(i => i.Component).HasForeignKey(i => i.ComponentId);
			modelBuilder.Entity<Component>()
				.HasMany(i => i.LLPData).WithOne(i => i.Component).HasForeignKey(i => i.ComponentId);
			modelBuilder.Entity<BaseComponent>()
				.HasMany(i => i.Regimes).WithOne(i => i.Component).HasForeignKey(i => i.EngineId);
			modelBuilder.Entity<Component>()
				.HasDiscriminator<bool>("IsBaseComponent")
				.HasValue<BaseComponent>(true)
				.HasValue<Component>(false);

			modelBuilder.Entity<AircraftFlight>()
				.HasMany(i => i.Regimes).WithOne(i => i.AircraftFlight).HasForeignKey(i => i.FlightId);

			modelBuilder.Entity<ComponentDirective>()
				.HasMany(i => i.PerformanceRecords).WithOne(i => i.ComponentDirective).HasForeignKey(i => i.ParentID);

			modelBuilder.Entity<MaintenanceDirective>()
				.HasMany(i => i.PerformanceRecords).WithOne(i => i.MaintenanceDirective).HasForeignKey(i => i.ParentID);

			modelBuilder.Entity<AircraftFlight>()
				.HasOne(i => i.CancelReason)
				.WithMany(i => i.AircraftFlightsCancels)
				.HasForeignKey(i => i.CancelReasonId);
		}

		#endregion

		#region public int GetIdFromQuery(string query)

		public async Task<int> GetIdFromQuery(string query)
		{
			var res = await IdQuery.FromSql(query).FirstOrDefaultAsync();
			return res?.Id ?? -1;
		}

		#endregion

		#region public int GetDestinationObjectIdQueryFromQuery(string query)

		public async Task<int> GetDestinationObjectIdQueryFromQuery(string query)
		{
			var res = await DestinationObjectIdQuery.FromSql(query).FirstOrDefaultAsync();
			return res?.Id ?? -1;
		}

		#endregion
	}
}