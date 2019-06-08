using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("Reasons", Schema = "Dictionaries")]
	public class Reason : BaseEntity
	{
		[Column("Name")]
		[MaxLength(50)]
		public string Name { get; set; }

		[Column("Category")]
		[MaxLength(50)]
		public string Category { get; set; }


		#region Navigation Property


		public ICollection<AircraftFlight> AircraftFlightsCancels { get; set; }

		#endregion
	}
}