using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entity
{
	[Table("Documents", Schema = "dbo")]
	public class Document : BaseEntity
	{
		[Column("RevisionValidTo")]
		public bool? RevisionValidTo { get; set; }

		[Column("IssueValidTo")]
		public bool? IssueValidTo { get; set; }

		[Column("IsClosed")]
		public bool? IsClosed { get; set; }

		[Column("IssueDateValidTo")]
		public DateTime IssueDateValidTo { get; set; }

		[Column("IssueNotify")]
		public int? IssueNotify { get; set; }

		[Column("RevisionDateValidTo")]
		public DateTime? RevisionDateValidTo { get; set; }

		[Column("RevisionNotify")]
		public int? RevisionNotify { get; set; }
	}
}