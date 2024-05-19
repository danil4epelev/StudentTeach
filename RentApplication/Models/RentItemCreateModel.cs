namespace RentApplication.Models
{
	public class RentItemCreateModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public long ChapterId { get; set; }
		public decimal Price { get; set; }
		public int PriceType { get; set; }
		public RentItemPropertyModel[] Propertes { get; set; }
	}

	public class RentItemPropertyModel
	{
		public long Id { get; set; }
		public object Value { get; set; }
	}
}
