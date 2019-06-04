using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Entity.Models;

namespace Entity.Entity
{
	public class BaseComponent : Component
	{
		[Column("BaseComponentTypeId")]
		public BaseComponentType BaseComponentTypeId { get; set; }

		public ICollection<EngineTimeInRegime> Regimes { get; set; }

	}
}
