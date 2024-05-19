using Rent.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rent.Core.Managers.Data
{
	public class ChapterData
	{
		public long Id { get; set; }
		public Guid Uid { get; set; }
		public string Name { get; set; }
		public long? ParentChapterId { get; set; }
	}
}
