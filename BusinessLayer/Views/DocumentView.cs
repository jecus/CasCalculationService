using System;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class DocumentView : BaseView
	{
		public bool RevisionValidTo { get; set; }

		public bool IssueValidTo { get; set; }

		public bool IsClosed { get; set; }

		public DateTime IssueDateValidTo { get; set; }

		public int IssueNotify { get; set; }

		public DateTime RevisionDateValidTo { get; set; }

		public int RevisionNotify { get; set; }

		public Lifelength Remains { get; set; }
		public Lifelength RevisionRemains { get; set; }
		public DateTime? NextPerformanceDate { get; set; }
		public ConditionState Condition { get; set; }


		public DocumentView(Document source)
		{
			if (source == null)
				return;
			Id = source.Id;
			RevisionValidTo = source.RevisionValidTo.Value;
			IssueValidTo = source.IssueValidTo.Value;
			IsClosed = source.IsClosed.Value;
			IssueDateValidTo = source.IssueDateValidTo;
			IssueNotify = source.IssueNotify.Value;
			RevisionDateValidTo = source.RevisionDateValidTo.Value;
			RevisionNotify = source.RevisionNotify.Value;
		}
	}
}