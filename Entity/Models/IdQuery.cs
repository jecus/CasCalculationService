using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
	public class IdQuery
	{
		[Column("ItemId")]
		public int Id { get; set; }
	}

	public class DestinationObjectIdQuery
	{
		[Column("DestinationObjectId")]
		public int Id { get; set; }
	}
}