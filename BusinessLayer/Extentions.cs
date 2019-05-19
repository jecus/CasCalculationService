using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public static class Extentions
	{
		public static ActualStateRecord GetLastKnownRecord(this ICollection<ActualStateRecord> records, DateTime date)
		{
			return records
				.OrderByDescending(i => i.RecordDate)
				.Where(i => i.RecordDate.HasValue && !i.IsDeleted)
				.FirstOrDefault(i => i.RecordDate.Value.Date <= date && i.FlightRegimeId == -1);
		}

		public static IQueryable<T> OnlyActive<T>(this IQueryable<T> queryable) where T :  BaseEntity
		{
			return queryable.Where(i => !i.IsDeleted);
		}
	}
}