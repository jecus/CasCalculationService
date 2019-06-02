using System.ComponentModel.DataAnnotations.Schema;
using Entity.Models;

namespace Entity.Entity
{
	public class RunUp
	{
		[NotMapped]
		public BaseComponent BaseComponent { get; set; }

		#region public Lifelength Lifelength { get; }
		/// <summary> 
		/// Возвращает наработку за данный пуск
		/// </summary>
		public Lifelength Lifelength
		{
			get { return new Lifelength(null, 1, 0 /*TotalMinutes*/); }
		}
		#endregion
	}
}