using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Vendors;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class DirectiveRecordView : BaseView
	{
		public int? NumGroup { get; set; }

		public int RecordTypeID { get; set; }

		public int? ParentID { get; set; }

		public int ParentTypeId { get; set; }

		public string Remarks { get; set; }

		public DateTime RecordDate { get; set; }

		public Lifelength OnLifelength { get; set; }

		public Lifelength Unused { get; set; }

		public Lifelength Overused { get; set; }

		public int? WorkPackageID { get; set; }

		public DateTime? Dispatched { get; set; }

		public bool? Completed { get; set; }

		public string Reference { get; set; }

		public bool? ODR { get; set; }

		public string MaintenanceOrganization { get; set; }

		public int? MaintenanceDirectiveRecordId { get; set; }

		public int? MaintenanceCheckRecordId { get; set; }

		public int PerformanceNum { get; set; }

		public bool IsControlPoint { get; set; }

		public Lifelength CalculatedPerformanceSource { get; set; }

		public string ComplianceCheckName { get; set; }


		public DirectiveRecordView(DirectiveRecord source)
		{
			Id = source.Id;
			NumGroup = source.NumGroup;
			RecordTypeID = source.RecordTypeID;
			ParentID = source.ParentID;
			ParentTypeId = source.ParentTypeId;
			Remarks = source.Remarks;
			RecordDate = source.RecordDate;
			WorkPackageID = source.WorkPackageID;
			Dispatched = source.Dispatched;
			Completed = source.Completed;
			Reference = source.Reference;
			ODR = source.ODR;
			MaintenanceOrganization = source.MaintenanceOrganization;
			MaintenanceDirectiveRecordId = source.MaintenanceDirectiveRecordId;
			MaintenanceCheckRecordId = source.MaintenanceCheckRecordId;
			PerformanceNum = source.PerformanceNum.Value;
			IsControlPoint = source.IsControlPoint;
			ComplianceCheckName = source.ComplianceCheckName;
			OnLifelength = Lifelength.ConvertFromByteArray(source.OnLifelength);
			Unused = Lifelength.ConvertFromByteArray(source.Unused);
			CalculatedPerformanceSource = Lifelength.ConvertFromByteArray(source.CalculatedPerformanceSource);
			Overused = Lifelength.ConvertFromByteArray(source.Overused);
		}

	}
}