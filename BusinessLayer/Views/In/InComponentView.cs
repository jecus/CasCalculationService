using System;
using Newtonsoft.Json;

namespace BusinessLayer.Views.In
{
	public class InComponentView
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int ComponentId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public DateTime EffectiveDate { get; set; }
	}
}