namespace RentApplication.Models
{
	public class ApprooveRegisterModel
	{
		public Guid ModelId { get; set; }
		public int ApproveCode { get; set; }
	}

	public class ApprooveResetPasswordModel : ApprooveRegisterModel
	{
		public string Password { get; set; }
	}
}
