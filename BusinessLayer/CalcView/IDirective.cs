using System.Collections.Generic;
using BusinessLayer.Vendors;
using BusinessLayer.Views;

namespace BusinessLayer.CalcView
{
	public interface IDirective
	{
		#region BaseView LifeLengthParent { get; }
		/// <summary>
		/// Возвращает родительский объект, для которого можно расчитать текущую наработку. Обычно Aircraft, BaseComponent или Component
		/// </summary>
		BaseView LifeLengthParent { get; }
		#endregion

		#region List<NextPerformance> NextPerformances { get; }
		/// <summary>
		/// Список последующих выполнений задачи
		/// </summary>
		List<NextPerformance> NextPerformances { get; }
		#endregion

		#region IThreshold Threshold { get; set; }
		/// <summary>
		/// Порог первого и посделующего выполнений
		/// </summary>
		IThreshold Threshold { get; set; }
		#endregion

		#region Boolean IsClosed { get; set; }
		///
		/// Логический флаг, показывающий, закрыта ли директива
		/// 
		bool IsClosed { get; set; }

		#endregion

		void ResetMathData();
	}
}