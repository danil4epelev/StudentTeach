using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers.Data
{
	public class UserData
	{
		public long Id { get; set; }
		public Guid Uid { get; set; }
		public string Login { get; set; }
		public string? Name { get; set; }
		public string? LastName { get; set; }
		public string? MiddleName { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
		public int RoleType { get; set; }
		public string PasswordHash { get; set; }
	}
}
