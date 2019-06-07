using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("Components", Schema = "dbo")]
	public class Component : BaseEntity
	{
		[NotMapped]
		public int AircaraftId { get; set; }

		[Column("PartNumber")]
		public string PartNumber { get; set; }
		
		[Column("ManufactureDate")]
		public DateTime? ManufactureDate { get; set; }

		[Column("IsBaseComponent")]
		public bool IsBaseComponent { get; set; }

		[Column("LLPMark")]
		public bool LLPMark { get; set; }

		[Column("LLPCategories")]
		public bool LLPCategories { get; set; }

		[Column("AverageUtilization")]
		public byte[] AverageUtilization { get; set; }

		public ICollection<ActualStateRecord> ActualStateRecords { get; set; }
		public ICollection<TransferRecord> TransferRecords { get; set; }
		public ICollection<ComponentLLPCategoryChangeRecord> ChangeLLPCategoryRecords { get; set; }
		public ICollection<ComponentDirective> ComponentDirectives { get; set; }
		public ICollection<ComponentLLPCategoryData> LLPData { get; set; }
	}
}