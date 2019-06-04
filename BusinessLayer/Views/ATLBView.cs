﻿using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ATLBView : BaseView
	{
		public int? AircraftID { get; set; }

		public string ATLBNo { get; set; }

		public ATLBView(ATLB source)
		{
			Id = source.Id;
			AircraftID = source.AircraftID;
			ATLBNo = source.ATLBNo;
		}
	}
}