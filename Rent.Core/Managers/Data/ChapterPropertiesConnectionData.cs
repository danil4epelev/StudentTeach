using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Managers.Data
{
	public class ChapterPropertiesConnectionData
	{
		public long Id { get; set; }
		public Guid Uid { get; set; }
		public long ChapterId { get; set; }
		public long PropertiesId { get; set; }
	}
}
