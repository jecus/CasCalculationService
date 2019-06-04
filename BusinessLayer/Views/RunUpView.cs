using Entity.Entity;
using Entity.Models;

namespace BusinessLayer.Views
{
	public class RunUpView : BaseView
	{
		public BaseComponentView BaseComponent { get; set; }

		#region public Lifelength Lifelength { get; }
		/// <summary> 
		/// Возвращает наработку за данный пуск
		/// </summary>
		public Lifelength Lifelength
		{
			get { return new Lifelength(null, 1, 0 /*TotalMinutes*/); }
		}
		#endregion

		public RunUpView(RunUp source)
		{
			Id = source.Id;
			BaseComponent = new BaseComponentView(source.BaseComponent);
		}
	}
}