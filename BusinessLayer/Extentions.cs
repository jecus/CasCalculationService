using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator.Views;
using BusinessLayer.Views;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public static class Extentions
	{
		public static ActualStateRecordView GetLastKnownRecord(this ICollection<ActualStateRecordView> records, DateTime date)
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

		public static ComponentLLPCategoryChangeRecordView GetLast(this ICollection<ComponentLLPCategoryChangeRecordView> Items)
		{
			return Items.Count == 0 ? null : Items.OrderBy(r => r.RecordDate).Last();
		}

		public static TransferRecordView[] GetRecords(this ICollection<TransferRecordView> items ,DateTime dateFrom, DateTime dateTo)
		{
			dateFrom = dateFrom.Date;
			dateTo = dateTo.Date;
			var array = items.ToArray();
			var res = new List<TransferRecordView>();

			// Если все записи сделаны раньше dateFrom
			if (array[array.Length - 1].TransferDate.Date <= dateFrom)
			{
				// В этом случае возвращаем только одну запись
				res.Add(array[array.Length - 1]);
				return res.ToArray();
			}

			return items.Where(i => i.TransferDate.Date >= dateFrom && i.TransferDate.Date <= dateTo).ToArray();
		}
	}
}