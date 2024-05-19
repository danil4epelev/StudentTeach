using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers.Data
{
	public class PropertiesData
	{
		public long Id { get; set; }
		public Guid Uid { get; set; }
		public int TypeProperties { get; set; }
		public string Name { get; set; }
		public string Values { get; set; }
		public bool IsRequired { get; set; }
	}
}
