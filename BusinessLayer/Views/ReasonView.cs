using Entity.Entity;

namespace BusinessLayer.Views
{
	public class ReasonView : BaseView
	{
		public string Name { get; set; }

		public string Category { get; set; }

		public ReasonView(Reason source)
		{
			if (source == null)
				return;
			Id = source.Id;
			Name = source.Name;
			Category = source.Category;
		}
	}
}