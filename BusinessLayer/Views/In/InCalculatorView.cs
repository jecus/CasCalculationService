using System;
using Newtonsoft.Json;

namespace BusinessLayer.Views.In
{
	public class InCalculatorView
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int AircraftId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DateFrom { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DateTo { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime CurrentDate { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int FlightId { get; set; }
	}
}