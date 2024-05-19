using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Entity
{
	public class Properties : BaseEntity
	{
		public int TypeProperties { get; set; }
		public string Name { get; set; }
		public string Values { get; set; }
		public bool IsRequired { get; set; }
	}
}
