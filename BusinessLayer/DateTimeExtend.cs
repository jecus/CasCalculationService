using System;
using Entity.Models;

namespace BusinessLayer.Calculator
{
	public static class DateTimeExtend
	{
		public static DateTime AddCalendarSpan(this DateTime dateTime, CalendarSpan calendarSpan)
		{
			if (calendarSpan == null)
				return new DateTime(dateTime.Ticks);

			DateTime result;
			switch (calendarSpan.CalendarType)
			{
				case CalendarTypes.Days:
					result = dateTime.AddDays(System.Convert.ToDouble(calendarSpan.CalendarValue));
					break;
				case CalendarTypes.Months:
					result = dateTime.AddMonths(System.Convert.ToInt32(calendarSpan.CalendarValue));
					break;
				case CalendarTypes.Years:
					result = dateTime.AddYears(System.Convert.ToInt32(calendarSpan.CalendarValue));
					break;
				default:
					result = new DateTime(dateTime.Ticks);
					break;
			}

			return result;
		}

		public static DateTime GetCASMinDateTime()
		{
			return new DateTime(1950, 1, 1);
		}
	}
}