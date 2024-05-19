using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rent.DataAccess.Entity
{
	public class Chapter : BaseEntity
	{
		public string Name { get; set; }
		public long? ParentChapterId { get; set; }
		public Chapter ParentChapter { get; set; }
	}
}
