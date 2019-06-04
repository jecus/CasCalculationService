using BusinessLayer.Views;

namespace BusinessLayer.Calculator.Dictionaries
{
	public class StaticDictionary : BaseView
	{
		#region public String ShortName { get; set; }
		/// <summary>
		/// Короткое имя
		/// </summary>
		public string ShortName { get; set; }

		#endregion

		#region public String FullName { get; set; }
		/// <summary>
		/// Полное имя
		/// </summary>
		public string FullName { get; set; }

		#endregion

		#region public String CommonName { get; set; }
		/// <summary>
		/// Общее имя 
		/// </summary>
		public string CommonName { get; set; }

		#endregion

		#region public override string ToString()
		/// <summary>
		/// Переводит тип директивы в строку
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FullName;
		}
		#endregion
	}
}