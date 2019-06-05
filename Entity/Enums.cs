namespace Entity
{
	public enum BaseComponentType
	{
		Propeller = 1,
		APU = 2,
		Engine = 3,
		Frame = 4,
		LandingGear = 5,
		Unknown = -1
	}

	public enum AtlbRecordType : short
	{
		/// <summary>
		/// Полет
		/// </summary>
		Flight = 1,
		/// <summary>
		/// Тех. обслуживание
		/// </summary>
		Maintenance = 2,
	}

	public enum LifelengthSubResource
	{
		/// <summary>
		/// минуты
		/// </summary>
		Minutes = 0,

		/// <summary>
		/// часы
		/// </summary>
		Hours = 1,

		/// <summary>
		/// Циклы
		/// </summary>
		Cycles = 2,

		/// <summary>
		/// Дни
		/// </summary>
		Calendar = 3,

	}

	public enum ThresholdConditionType
	{
		/// <summary>
		/// Директива выполнится при выполнении одного из условий
		/// </summary>
		WhicheverFirst = 0,
		/// <summary>
		/// Директива выполнится при выполнении всех условий 
		/// </summary>
		WhicheverLater = 1,
	}

	public enum SmartCoreType
	{
		Aircraft = 7,
	}
}