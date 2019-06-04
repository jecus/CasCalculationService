using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Calculator.Dictionaries;
using BusinessLayer.Calculator.Views;
using BusinessLayer.Views;
using Entity.Entity;

namespace BusinessLayer.Calculator
{
	public static class Extentions
	{
		public static ActualStateRecordView GetLastKnownRecord(this List<ActualStateRecordView> records, DateTime date)
		{
			return records
				.OrderByDescending(i => i.RecordDate)
				.Where(i => i.RecordDate.HasValue && !i.IsDeleted)
				.FirstOrDefault(i => i.RecordDate.Value.Date <= date && i.FlightRegimeId == -1);
		}

		public static ActualStateRecordView GetLastKnownRecord(this List<ActualStateRecordView> records, DateTime date, FlightRegime flightRegime)
		{
			if (flightRegime == null) flightRegime = FlightRegime.UNK;
			date = date.Date;
			return records.OrderByDescending(r => r.RecordDate.Value.Date)
				.FirstOrDefault(r => r.RecordDate.Value.Date <= date && r.WorkRegimeTypeId.Equals(flightRegime.Id));
		}

		public static IEnumerable<RunUpView> GetByBaseComponent(this List<RunUpView> records, BaseComponentView bd)
		{
			List<RunUpView> runups = new List<RunUpView>();

			if (bd != null) runups.AddRange(records.Where(r => r.BaseComponent.Id == bd.Id));

			return runups;
		}

		public static IQueryable<T> OnlyActive<T>(this IQueryable<T> queryable) where T :  BaseEntity
		{
			return queryable.Where(i => !i.IsDeleted);
		}

		public static ComponentLLPCategoryChangeRecordView GetLast(this ICollection<ComponentLLPCategoryChangeRecordView> Items)
		{
			return Items.Count == 0 ? null : Items.OrderBy(r => r.RecordDate).Last();
		}

		public static TransferRecordView[] GetRecords(this List<TransferRecordView> items ,DateTime dateFrom, DateTime dateTo)
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

		#region public T[] GetRecords(DateTime toDate)
		/// <summary>
		/// Возвращает записи до указанной даты
		/// </summary>
		/// <param name="toDate"></param>
		/// <returns></returns>
		public static TransferRecordView[] GetRecords(this List<TransferRecordView> items , DateTime toDate)
		{
			//
			List<TransferRecordView> res;
			toDate = toDate.Date;
			var array = items.ToArray();
			// Если последняя запись о перемещении входит в запрошенный диапазон возвращаем все перемещения
			if (array.Length > 0 && array[array.Length - 1].TransferDate.Date <= toDate)
			{
				return array.ToArray();
			}

			// Находим последнее перемещение удовлетворяющее запрошенному интервалу
			for (int i = array.Length - 1; i >= 0; i--)
				if (array[i].TransferDate.Date <= toDate)
				{
					res = new List<TransferRecordView>();
					for (int j = 0; j <= i; j++)
						res.Add(array[j]);
					return res.ToArray();
				}

			//
			return null;
		}
		#endregion

	}
}