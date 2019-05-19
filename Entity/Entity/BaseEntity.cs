using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	public class BaseEntity
	{
		[Column("ItemId")]
		public int Id { get; set; }

		[Column("IsDeleted")]
		public bool IsDeleted { get; set; }
	}
}