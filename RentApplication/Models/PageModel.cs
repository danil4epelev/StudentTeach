namespace RentApplication.Models
{
	public class PageModel<T>
	{
		public long Total { get; set; }

		public IEnumerable<T> Data { get; set; }
	}
}
