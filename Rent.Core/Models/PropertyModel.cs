using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Models
{
	public class PropertyModel
	{
		public string Name { get; set; }
		public int TypeProperties { get; set; }
		public string[] Values { get; set; }
		public bool IsRequired { get; set; }
	}
}
