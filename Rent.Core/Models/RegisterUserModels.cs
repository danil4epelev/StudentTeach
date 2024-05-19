using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Models
{
	public class BaseUserModel
	{
		public string Email { get; set; }
	}

	public class RegisterUserModels : BaseUserModel
	{
		public string Password { get; set; }
	}

	public class RegisterUserModelsExpanded : RegisterUserModels
	{
		public int ApproveCode { get; set; }
	}

	public class LoginUserModels
	{
		public string Login { get; set; }
		public string Password { get; set; }
	}
}
