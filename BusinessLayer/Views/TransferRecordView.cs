﻿using System;
using Entity.Entity;

namespace BusinessLayer.Views
{
	public class TransferRecordView : BaseView
	{
		public int? ParentID { get; set; }

		public DateTime TransferDate { get; set; }

		public int? DestinationObjectId { get; set; }

		public int? DestinationObjectType { get; set; }

		public string Position { get; set; }


		public TransferRecordView(TransferRecord source)
		{
			Id = source.Id;
			ParentID = source.ParentID;
			TransferDate = source.TransferDate.Value;
			Position = source.Position;
			DestinationObjectId = source.DestinationObjectId;
			DestinationObjectType = source.DestinationObjectType;
		}
	}
}