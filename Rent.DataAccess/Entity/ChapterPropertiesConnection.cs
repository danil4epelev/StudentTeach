using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.DataAccess.Entity
{
	public class ChapterPropertiesConnection : BaseEntity
	{
		public long ChapterId { get; set; }
		public long PropertiesId { get; set; }
	}
}
