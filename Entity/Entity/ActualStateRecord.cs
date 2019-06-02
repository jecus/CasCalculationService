using System;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Models;

namespace Entity.Entity
{
	[Table("ActualStateRecords", Schema = "dbo")]
	public class ActualStateRecord : BaseEntity
	{
		[Column("FlightRegimeId")]
		public int FlightRegimeId { get; set; }

		[Column("Remarks")]
		public string Remarks { get; set; }

		[Column("OnLifelength")]
		public byte[] OnLifelengthByte { get; set; }

		[NotMapped] public Lifelength OnLifelength => Lifelength.ConvertFromByteArray(OnLifelengthByte);


		[Column("RecordDate")]
		public DateTime? RecordDate { get; set; }

		[Column("WorkRegimeTypeId")]
		public int? WorkRegimeTypeId { get; set; }

		[Column("ComponentId")]
		public int? ComponentId { get; set; }

		#region Navigation Property

		public Component Component { get; set; }

		#endregion
	}
}