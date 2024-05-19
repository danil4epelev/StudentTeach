namespace RentApplication.Models
{
	public class FilterModelBase
	{
		public long? StartFilterElement { get; set; }
	}

	public class FilterModel : FilterModelBase
	{
		public DateTime? MaxCreateDt { get; set; }
		public DateTime? MinCreateDt { get; set; }
		public decimal? MaxPrice { get; set; }
		public decimal? MinPrice { get; set; }
	}
}
