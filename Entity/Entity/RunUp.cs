using System.ComponentModel.DataAnnotations.Schema;
using Entity.Models;

namespace Entity.Entity
{
	public class RunUp : BaseEntity
	{
		[NotMapped]
		public BaseComponent BaseComponent { get; set; }

	}
}