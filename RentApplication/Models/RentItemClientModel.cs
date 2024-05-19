namespace RentApplication.Models
{
	public class RentItemClientModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int PriceType { get; set; }
	}
}
