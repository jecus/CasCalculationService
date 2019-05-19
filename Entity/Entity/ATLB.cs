using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Entity.Entity
{
	[Table("ATLBs", Schema = "dbo")]
	public class ATLB : BaseEntity
	{
		[Column("AircraftID")]
		public int? AircraftID { get; set; }

		[Column("ATLBNo")]
		public string ATLBNo { get; set; }
	}
}