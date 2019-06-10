using System.Collections.Generic;
using BusinessLayer.Views;
using Entity.Entity;

namespace CalculationService
{
	public static class GlobalObjects
	{
		private static Dictionary<int, List<AircraftFlightView>> _flights;

		public static Dictionary<int, List<AircraftFlightView>> Flights
		{
			get => _flights ?? (_flights = new Dictionary<int, List<AircraftFlightView>>());
			set => _flights = value;
		}
	}
}