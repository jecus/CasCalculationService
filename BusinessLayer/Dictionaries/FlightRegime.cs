using System;
using System.Collections.Generic;

namespace BusinessLayer.Calculator.Dictionaries
{
	public class FlightRegime : StaticDictionary
	{
		#region private static CommonDictionaryCollection<FlightRegime> _Items = new CommonDictionaryCollection<FlightRegime>();
		/// <summary>
		/// Содержит все элементы
		/// </summary>
		private static List<FlightRegime> _Items = new List<FlightRegime>();
		#endregion

		/*
         * Предопределенные типы
         */

		#region public static FlightRegime SmallGas = new FlightRegime(1, "SG", "Small Gas");
		/// <summary>
		/// Малый газ
		/// </summary>
		public static FlightRegime SmallGas = new FlightRegime(1, "SG", "Small Gas");
		#endregion

		#region public static FlightRegime RunUp = new FlightRegime(2, "RU", "Run up");
		/// <summary>
		/// Запуск
		/// </summary>
		public static FlightRegime RunUp = new FlightRegime(2, "RU", "Run up");
		#endregion

		#region public static FlightRegime Nominal = new FlightRegime(10, "N", "Nominal");
		/// <summary>
		/// Номинальный
		/// </summary>
		public static FlightRegime Nominal = new FlightRegime(10, "N", "Nominal");
		#endregion

		#region public static FlightRegime Nominal85 = new FlightRegime(11, "N085", "0.85 Nominal");
		/// <summary>
		/// 85% Номинальный
		/// </summary>
		public static FlightRegime Nominal85 = new FlightRegime(11, "N085", "0.85 Nominal");
		#endregion

		#region public static FlightRegime Nominal70 = new FlightRegime(12, "N070", "0.7 Nominal");
		/// <summary>
		/// 70% Номинальный
		/// </summary>
		public static FlightRegime Nominal70 = new FlightRegime(12, "N070", "0.7 Nominal");
		#endregion

		#region public static FlightRegime Nominal60 = new FlightRegime(13, "N060", "0.6 Nominal");
		/// <summary>
		/// 60% Номинальный
		/// </summary>
		public static FlightRegime Nominal60 = new FlightRegime(13, "N060", "0.6 Nominal");
		#endregion

		#region public static FlightRegime Nominal40 = new FlightRegime(14, "N040", "0.4 Nominal");
		/// <summary>
		/// 40% Номинальный
		/// </summary>
		public static FlightRegime Nominal40 = new FlightRegime(14, "N040", "0.4 Nominal");
		#endregion

		#region public static FlightRegime Nominal20 = new FlightRegime(15, "N020", "0.2 Nominal");
		/// <summary>
		/// 20% Номинальный
		/// </summary>
		public static FlightRegime Nominal20 = new FlightRegime(15, "N020", "0.2 Nominal");
		#endregion

		#region public static FlightRegime TakeOff = new FlightRegime(20, "TO", "Take Off");
		/// <summary>
		/// Взлетный
		/// </summary>
		public static FlightRegime TakeOff = new FlightRegime(20, "TO", "Take Off");
		#endregion

		#region public static FlightRegime Cruise = new FlightRegime(30, "Cr", "Cruise");
		/// <summary>
		/// Взлетный
		/// </summary>
		public static FlightRegime Cruise = new FlightRegime(30, "Cr", "Cruise");
		#endregion

		#region public static FlightRegime UNK = new FlightRegime(-1, "UNK", "Unknown");
		/// <summary>
		/// неизвестный груз
		/// </summary>
		public static FlightRegime UNK = new FlightRegime(-1, "UNK", "Unknown");
		#endregion

		/*
         * Свойства 
         */

		/*
         * Методы
         */

		#region public static FlightRegime GetItemById(Int32 maintenanceTypeId)
		/// <summary>
		/// Возвращает тип диерктивы по его Id
		/// </summary>
		/// <param name="maintenanceTypeId"></param>
		/// <returns></returns>
		public static FlightRegime GetItemById(Int32 maintenanceTypeId)
		{
			foreach (FlightRegime t in _Items)
				if (t.Id == maintenanceTypeId)
					return t;
			//
			return UNK;
		}

		#endregion

		#region static public CommonDictionaryCollection<FlightRegime> Items
		/// <summary>
		/// Возвращает список  элементов коллекции
		/// </summary>
		public static List<FlightRegime> Items
		{
			get
			{
				return _Items;
			}
		}
		#endregion

		#region public override string ToString()
		/// <summary>
		/// Переводит тип директивы в строку
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FullName;
		}
		#endregion

		/*
         * Реализация
         */
		#region public FlightRegime()
		/// <summary>
		/// Конструктор создает объект типа директивы
		/// </summary>
		public FlightRegime()
		{
			
		}
		#endregion

		#region public FlightRegime(Int32 itemId, String shortName, String fullName) : this()
		/// <summary>
		/// Конструктор создает объект типа директивы
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="shortName"></param>
		/// <param name="fullName"></param>
		public FlightRegime(Int32 itemId, String shortName, String fullName) : this()
		{
			Id = itemId;
			ShortName = shortName;
			FullName = fullName;

			_Items.Add(this);
		}
		#endregion
		
	}
}