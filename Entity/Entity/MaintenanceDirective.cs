using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("MaintenanceDirectives", Schema = "dbo")]
	public class MaintenanceDirective : BaseEntity
	{
		[Column("Threshold")]
		[MaxLength(116)]
		public byte[] Threshold { get; set; }

		public ICollection<DirectiveRecord> PerformanceRecords { get; set; }
	}
}