using System;
using System.Globalization;
using Entity;
using Entity.Models;

namespace BusinessLayer.CalcView
{
	[Serializable]
	public class AverageUtilization
	{
		/*
		* Свойства 
		*/

		#region public Double CyclesPerDay { get; set; }
		/// <summary>
		/// Среднестатистический дневной налет (циклы)
		/// </summary>
		public Double CyclesPerDay
		{
			get
			{
				return _cyclesPerMonth / 30;
			}
			set
			{
				_cyclesPerMonth = value * 30;
			}
		}
		#endregion

		#region public Double HoursPerDay { get; set; }
		/// <summary>
		/// Среднестатистический дневной налет (часы)
		/// </summary>
		public Double HoursPerDay
		{
			get
			{
				return _hoursPerMonth / 30;
			}
			set
			{
				_hoursPerMonth = value * 30;
			}
		}
		#endregion

		#region public Double CyclesPerMonth { get; set; }
		/// <summary>
		/// Среднестатистический месячный налет ВС
		/// </summary>
		private Double _cyclesPerMonth;
		/// <summary>
		/// Среднестатистический месячный налет ВС (циклы)
		/// </summary>
		public Double CyclesPerMonth
		{
			get { return _cyclesPerMonth; }
			set { _cyclesPerMonth = value; }
		}
		#endregion

		#region public Double HoursPerMonth { get; set; }
		/// <summary>
		/// Среднестатистический месячный налет ВС (часы)
		/// </summary>
		private Double _hoursPerMonth;
		/// <summary>
		/// Среднестатистический месячный налет ВС (часы)
		/// </summary>
		public Double HoursPerMonth
		{
			get { return _hoursPerMonth; }
			set { _hoursPerMonth = value; }
		}
		#endregion

		// Поддержка старой системы

		#region public Double Cycles { get; }
		/// <summary>
		/// В зависимости от Selected Interval возвращает либо Cycles Per Day либо Cycles Per Month
		/// </summary>
		public Double Cycles
		{
			get { return SelectedInterval == UtilizationInterval.Monthly ? CyclesPerMonth : CyclesPerDay; }
		}
		#endregion

		#region public Double Hours { get; }
		/// <summary>
		/// В зависимости от Selected Interval возвращает либо Hours Per Day либо Hours Per Month
		/// </summary>
		public Double Hours
		{
			get { return SelectedInterval == UtilizationInterval.Monthly ? HoursPerMonth : HoursPerDay; }
		}
		#endregion

		#region public UtilizationInterval SelectedInterval { get; set; }
		/// <summary>
		/// Выбранный интервал среднестатистических данных
		/// </summary>
		public UtilizationInterval SelectedInterval { get; set; }
		#endregion

		#region public static int SerializedDataLength { get; }
		/// <summary>
		/// Gets length of any serialized
		/// </summary>
		public static int SerializedDataLength
		{
			get { return 20; }
		}
		#endregion

		/*
         * Конструктор
         */
		#region public AverageUtilization()
		/// <summary>
		/// Создает экземпляр класса без параметров
		/// </summary>
		public AverageUtilization()
		{
			_cyclesPerMonth = 0;
			_hoursPerMonth = 0;
		}
		#endregion

		#region public AverageUtilization(Double cyclesPerMonth, Double hoursPerMonth)
		/// <summary>
		/// Создает экземпляр класса 
		/// </summary>
		/// <param name="cyclesPerMonth"></param>
		/// <param name="hoursPerMonth"></param>
		public AverageUtilization(Double cyclesPerMonth, Double hoursPerMonth)
		{
			_cyclesPerMonth = cyclesPerMonth;
			_hoursPerMonth = hoursPerMonth;
			SelectedInterval = UtilizationInterval.Monthly;
		}
		#endregion

		#region public AverageUtilization(Double cycles, Double hours, UtilizationInterval interval)
		/// <summary>
		/// Создает экземпляр класса Average Utilization в зависимости от выбранного интервала среднестатической наработки
		/// </summary>
		/// <param name="cycles"></param>
		/// <param name="hours"></param>
		/// <param name="interval"></param>
		public AverageUtilization(Double cycles, Double hours, UtilizationInterval interval)
		{
			SelectedInterval = interval;
			//
			if (interval == UtilizationInterval.Dayly)
			{
				CyclesPerDay = cycles;
				HoursPerDay = hours;
			}
			else
			{
				CyclesPerMonth = cycles;
				HoursPerMonth = hours;
			}
		}
		#endregion

		/*
         * Методы
         */
		#region public static AverageUtilization ConvertFromByteArray(byte[] data)
		/// <summary>
		/// Конвертирует данные из БД в AverageUtilization
		/// </summary>
		/// <param name="data"></param>
		public static AverageUtilization ConvertFromByteArray(byte[] data)
		{

			AverageUtilization item = new AverageUtilization();

			if (data == null)
			{
				return item;
			}

			byte[] binaryData = data;
			if (null == binaryData) return item;

			if (binaryData == null || binaryData.Length != SerializedDataLength)
				throw new ArgumentException("Data cannot be converted to Lifelength");

			item.SelectedInterval = (UtilizationInterval)DbTypes.Int32FromByteArray(binaryData, 0);

			item._hoursPerMonth = BitConverter.ToDouble(binaryData, 4);
			item._cyclesPerMonth = BitConverter.ToDouble(binaryData, 12);
			return item;
		}

		#endregion

		#region public override string ToString()
		/// <summary>
		/// Представляет в виде строки 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			const string specifier = "G3";
			CultureInfo culture = CultureInfo.CreateSpecificCulture("eu-ES");
			string interval = SelectedInterval == UtilizationInterval.Monthly ? "MONTH" : "DAY";
			return string.Format("FH/{0}:{1} FC/{0}:{2}", interval, Hours.ToString(specifier, culture), Cycles.ToString(specifier, culture));
		}
		#endregion
	}
}