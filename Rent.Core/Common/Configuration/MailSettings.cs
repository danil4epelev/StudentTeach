using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Common.Configuration
{
	public class MailSettings
	{
		public string Host { get; set; }
		public bool IsSendMailWithSSL { get; set; }
		public string Domain { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int Port { get; set; }
	}
}
