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

	public enum SmartCoreType
	{
		Aircraft = 7,
	}
}