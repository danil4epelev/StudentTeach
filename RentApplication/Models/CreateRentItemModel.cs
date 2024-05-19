namespace RentApplication.Models
{
	public class CreateRentItemModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public long ChapterId { get; set; }
		public string Properties { get; set; }
		public decimal Price { get; set; }
		public int PriceType { get; set; }
	}
}
