using System.Linq;
using Entity.Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
	public class DatabaseContext : DbContext
	{
		public DbSet<AircraftFlight> AircraftFlights { get; set; }
		public DbSet<ATLB> Atlbs { get; set; }
		public DbSet<Aircraft> Aircrafts { get; set; }
		public DbSet<Component> Components { get; set; }
		public DbSet<ActualStateRecord> ActualStateRecords { get; set; }
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
				.HasMany(i => i.ActualStateRecords).WithOne(i => i.Component).HasForeignKey(i => i.ComponentId);
		}

		#endregion

		public int GetIdFromQuery(string query)
		{
			return IdQuery.FromSql(query).FirstOrDefault()?.Id ?? -1;
		}
		public int GetDestinationObjectIdQueryFromQuery(string query)
		{
			return DestinationObjectIdQuery.FromSql(query).FirstOrDefault()?.Id ?? -1;
		}
	}
}