using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Entity.Models;

namespace Entity.Entity
{
	[Table("ComponentLLPCategoryChangeRecords", Schema = "dbo")]
	public class ComponentLLPCategoryChangeRecord : BaseEntity
	{
		[Column("ParentId")]
		public int? ParentId { get; set; }

		[Column("RecordDate")]
		public DateTime? RecordDate { get; set; }

		[Column("OnLifeLength")]
		[MaxLength(50)]
		public byte[] OnLifeLengthByte { get; set; }

		#region Navigation Property

		public Component Component { get; set; }

		#endregion
	}
}