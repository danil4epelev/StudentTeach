using Rent.Core.Managers.Data;
using Rent.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Contracts.Helpers
{
	public interface IChapterHelper
	{
		public PropertiesData[] GetAllTreePropertiesByChapter(long chapterId);
	}
}
