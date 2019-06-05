using System;
using BusinessLayer.Vendors;
using Entity;
using Entity.Models;

namespace BusinessLayer.CalcView
{
	public class Threshold : IThreshold
	{
		#region Implementation of IThreshold

		public ThresholdConditionType FirstPerformanceConditionType { get; set; }
		public ThresholdConditionType RepeatPerformanceConditionType { get; set; }
		public bool PerformRepeatedly { get; set; }
		public DateTime EffectiveDate { get; set; }
		public Lifelength FirstPerformanceSinceNew { get; set; }
		public Lifelength FirstPerformanceSinceEffectiveDate { get; set; }
		public Lifelength FirstNotification { get; set; }
		public Lifelength RepeatInterval { get; set; }
		public Lifelength RepeatNotification { get; set; }
		public Lifelength Warranty { get; set; }
		public Lifelength WarrantyNotification { get; set; }

		#endregion



		#region public static void ConvertFromSerializedData(ref byte[] data, int serializedDataLength)

		public static int SerializedDataLengthComponentDirective
		{
			get
			{
				return sizeof(bool) * 2 + Lifelength.SerializedDataLength * 6;
			}
		}

		/// <summary>
		/// Получает свойства из массива байт
		/// </summary>
		/// <param name="data"></param>
		/// <param name="serializedDataLength"></param>
		public static Threshold ConvertForComponentDirective(byte[] data)
		{
			var  item = new Threshold();

			if (data == null) data = new byte[SerializedDataLengthComponentDirective];
			var serializedFirstPerformance = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, 0, serializedFirstPerformance, 0, Lifelength.SerializedDataLength);

			item.FirstPerformanceSinceNew = Lifelength.ConvertFromByteArray(serializedFirstPerformance);

			var serializedFirstNotification = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, Lifelength.SerializedDataLength, serializedFirstNotification, 0, Lifelength.SerializedDataLength);

			item.FirstNotification = Lifelength.ConvertFromByteArray(serializedFirstNotification);

			var serializedrepeatPerform = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, Lifelength.SerializedDataLength * 2, serializedrepeatPerform, 0, Lifelength.SerializedDataLength);

			item.RepeatInterval = Lifelength.ConvertFromByteArray(serializedrepeatPerform);

			var serializedNotification = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, Lifelength.SerializedDataLength * 3, serializedNotification, 0, Lifelength.SerializedDataLength);

			item.RepeatNotification = Lifelength.ConvertFromByteArray(serializedNotification);

			var serializedWarranty = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, Lifelength.SerializedDataLength * 4, serializedWarranty, 0, Lifelength.SerializedDataLength);

			item.Warranty = Lifelength.ConvertFromByteArray(serializedWarranty);

			var serializedWarrantyNotification = new byte[Lifelength.SerializedDataLength];
			Array.Copy(data, Lifelength.SerializedDataLength * 5, serializedWarrantyNotification, 0, Lifelength.SerializedDataLength);

			item.WarrantyNotification = Lifelength.ConvertFromByteArray(serializedWarrantyNotification);

			item.PerformRepeatedly = data[Lifelength.SerializedDataLength * 6] == 1;

			item.FirstPerformanceConditionType = data[Lifelength.SerializedDataLength * 6 + 1] == 1 ? ThresholdConditionType.WhicheverLater : ThresholdConditionType.WhicheverFirst;

			// если еще есть данные то рекурсивно считываем и его
			var dataIndex = SerializedDataLengthComponentDirective;
			var dataLeft = data.Length - dataIndex;

			if (dataLeft >= sizeof(long))
			{
				var serializedEffectivityDate = new byte[sizeof(long)];
				Array.Copy(data, dataIndex, serializedEffectivityDate, 0, sizeof(long));

				item.EffectiveDate = new DateTime(DbTypes.Int64FromByteArray(serializedEffectivityDate, 0));
			}

			dataLeft -= sizeof(long);
			dataIndex += sizeof(long);

			if (dataLeft >= Lifelength.SerializedDataLength)
			{
				var serializedRepeatNotificaction = new byte[Lifelength.SerializedDataLength];
				Array.Copy(data, dataIndex, serializedRepeatNotificaction, 0, Lifelength.SerializedDataLength);

				item.FirstPerformanceSinceEffectiveDate = Lifelength.ConvertFromByteArray(serializedRepeatNotificaction);
			}

			return item;
		}
		#endregion
	}
}