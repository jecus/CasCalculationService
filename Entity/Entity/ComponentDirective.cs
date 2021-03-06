﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("ComponentDirectives", Schema = "dbo")]
	public class ComponentDirective : BaseEntity
	{
		[Column("DirectiveType")]
		public int DirectiveType { get; set; }

		[Column("Threshold")]
		[MaxLength(200)]
		public byte[] Threshold { get; set; }

		[Column("ComponentId")]
		public int? ComponentId { get; set; }

		[Column("IsClosed")]
		public bool IsClosed { get; set; }


		public ICollection<DirectiveRecord> PerformanceRecords { get; set; }

		#region Navigation Property

		public Component Component { get; set; }

		#endregion
	}
}