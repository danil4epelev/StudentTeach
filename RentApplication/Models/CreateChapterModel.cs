using Rent.Core.Models;

namespace RentApplication.Models
{
	public class UpdateChapterModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public long? ParentId { get; set; }
	}

	public class CreateChapterModel 
	{
		public long[] PropertiesIds { get; set; }
		public string Name { get; set; }
		public long? ParentId { get; set; }
	}

	
}
