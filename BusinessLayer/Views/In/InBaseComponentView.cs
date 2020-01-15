using System;
using Newtonsoft.Json;

namespace BusinessLayer.Views.In
{
	public class InBaseComponentView
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int BaseComponentId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int FlightId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime Date { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime EffectiveDate { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int FlightRegimeId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime FromDate { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime ToDate { get; set; }

	}
}