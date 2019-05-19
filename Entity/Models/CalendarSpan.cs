using System;

namespace Entity.Models
{
	public class CalendarSpan
	{
		private int? _calendarValue;
		private CalendarTypes _calendarType;

		public int? CalendarValue
		{
			get { return _calendarValue; }
			set { _calendarValue = value; }
		}

		public CalendarTypes CalendarType
		{
			get { return _calendarType; }
			set { _calendarType = value; }
		}

		#region public int? Days

		public int? Days
		{
			get
			{
				if (_calendarValue == null)
					return null;

				int value;
				switch (_calendarType)
				{
					case CalendarTypes.Months:
						value = (int)Math.Round(Convert.ToDouble(_calendarValue * 30.4375));
						break;
					case CalendarTypes.Years:
						value = (int)Math.Round(Convert.ToDouble(_calendarValue * 365.25));
						break;
					default:
						value = Convert.ToInt32(_calendarValue);
						break;
				}
				return value;
			}
		}
		#endregion

		#region public int? Months

		public int? Months
		{
			get
			{
				if (_calendarValue == null)
					return null;

				int value;
				switch (_calendarType)
				{
					case CalendarTypes.Months:
						value = Convert.ToInt32(_calendarValue);
						break;
					case CalendarTypes.Years:
						value = Convert.ToInt32(_calendarValue * 12);
						break;
					default:
						value = (int)Math.Round(Convert.ToDouble(_calendarValue / 30.4375));
						break;
				}
				return value;
			}
		}
		#endregion

		#region public int? Years

		public int? Years
		{
			get
			{
				if (_calendarValue == null)
					return null;

				int value;
				switch (_calendarType)
				{
					case CalendarTypes.Months:
						value = (int)Math.Round(Convert.ToDouble(_calendarValue / 12));
						break;
					case CalendarTypes.Years:
						value = Convert.ToInt32(_calendarValue);
						break;
					default:
						value = (int)Math.Round(Convert.ToDouble(_calendarValue / 365.25));
						break;
				}
				return value;
			}
		}
		#endregion

		#region public CalendarSpan()

		public CalendarSpan()
		{
			_calendarValue = null;
			_calendarType = CalendarTypes.Days;
		}
		#endregion

		#region public CalendarSpan(int? calandarValue, CalendarTypes calendarType)

		public CalendarSpan(int? calandarValue, CalendarTypes calendarType)
		{
			_calendarValue = calandarValue;
			_calendarType = calendarType;
		}
		#endregion

		#region public CalendarSpan(CalendarSpan source)
		public CalendarSpan(CalendarSpan source)
		{
			if (source != null)
			{
				_calendarValue = source.CalendarValue;
				_calendarType = source.CalendarType;
			}
			else
			{
				_calendarValue = null;
				_calendarType = CalendarTypes.Days;
			}
		}
		#endregion

		/*
         * Operator
         */

		#region public static bool operator > (CalendarSpan a, CalendarSpan b)

		public static bool operator >(CalendarSpan a, CalendarSpan b)
		{
			if (a == null || a.CalendarValue == null)
				return false;
			if (b == null || b.CalendarValue == null)
				return true;
			if (a.CalendarType == b.CalendarType)
			{
				return a.CalendarValue > b.CalendarValue;
			}

			double aValue, bValue;

			switch (a.CalendarType)
			{
				case CalendarTypes.Months:
					aValue = Math.Round(System.Convert.ToDouble(a.CalendarValue * 30.4375));
					break;
				case CalendarTypes.Years:
					aValue = Math.Round(System.Convert.ToDouble(a.CalendarValue * 365.25));
					break;
				default:
					aValue = System.Convert.ToDouble(a.CalendarValue);
					break;
			}

			switch (b.CalendarType)
			{
				case CalendarTypes.Months:
					bValue = Math.Round(System.Convert.ToDouble(b.CalendarValue * 30.4375));
					break;
				case CalendarTypes.Years:
					bValue = Math.Round(System.Convert.ToDouble(b.CalendarValue * 365.25));
					break;
				default:
					bValue = System.Convert.ToDouble(b.CalendarValue);
					break;
			}

			return aValue > bValue;
		}
		#endregion

		#region public static bool operator < (CalendarSpan a, CalendarSpan b)

		public static bool operator <(CalendarSpan a, CalendarSpan b)
		{
			if (b == null || b.CalendarValue == null)
				return false;
			if (a == null || a.CalendarValue == null)
				return true;
			if (a.CalendarType == b.CalendarType)
			{
				return a.CalendarValue < b.CalendarValue;
			}

			double aValue, bValue;

			if (a.CalendarType == CalendarTypes.Months)
				aValue = Math.Round(System.Convert.ToDouble(a.CalendarValue * 30.4375));
			else if (a.CalendarType == CalendarTypes.Years)
				aValue = Math.Round(System.Convert.ToDouble(a.CalendarValue * 365.25));
			else aValue = System.Convert.ToDouble(a.CalendarValue);

			if (b.CalendarType == CalendarTypes.Months)
				bValue = Math.Round(System.Convert.ToDouble(b.CalendarValue * 30.4375));
			else if (b.CalendarType == CalendarTypes.Years)
				bValue = Math.Round(System.Convert.ToDouble(b.CalendarValue * 365.25));
			else bValue = System.Convert.ToDouble(b.CalendarValue);

			return aValue < bValue;
		}
		#endregion
	}
}