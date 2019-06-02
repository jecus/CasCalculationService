using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("TransferRecords", Schema = "dbo")]
	public class TransferRecord : BaseEntity
	{
		[Column("ParentID")]
		public int? ParentID { get; set; }

		[Column("TransferDate")]
		public DateTime? TransferDate { get; set; }

		[Column("DestinationObjectID")]
		public int? DestinationObjectId { get; set; }

		[Column("DestinationObjectType")]
		public int? DestinationObjectType { get; set; }

		#region Navigation Property

		public Component Component { get; set; }

		#endregion
	}
}