using System.Collections.Generic;
using BusinessLayer.Views;
using Entity.Entity;

namespace CalculationService
{
	public static class GlobalObjects
	{
		private static Dictionary<int, List<AircraftFlightView>> _flights;
		private static List<BaseComponentView> _baseComponents;
		private static List<AircraftView> _aircrafts;

		public static List<AircraftView> Aircrafts
		{
			get => _aircrafts ?? (_aircrafts = new List<AircraftView>());
			set => _aircrafts = value;
		}

		public static Dictionary<int, List<AircraftFlightView>> Flights
		{
			get => _flights ?? (_flights = new Dictionary<int, List<AircraftFlightView>>());
			set => _flights = value;
		}

		public static List<BaseComponentView> BaseComponents
		{
			get => _baseComponents ?? (_baseComponents = new List<BaseComponentView>());
			set => _baseComponents = value;
		}
	}
}