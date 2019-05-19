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
}