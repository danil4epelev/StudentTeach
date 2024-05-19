using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers.Data;
using Rent.Core.Models;
using Rent.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rent.Core.Helpers
{
	public class ChapterHelper : IChapterHelper
	{
		private readonly IChapterManager _chapterManager;
		private readonly IChapterPropertiesConnectionManager _chapterPropertiesConnectionManager;
		private readonly IPropertiesManager _propertiesManager;

		public ChapterHelper(IChapterManager chapterManager,
			IChapterPropertiesConnectionManager chapterPropertiesConnectionManager,
			IPropertiesManager propertiesManager)
		{
			_chapterManager = chapterManager;
			_propertiesManager = propertiesManager;
			_chapterPropertiesConnectionManager = chapterPropertiesConnectionManager;
		}

		public PropertiesData[] GetAllTreePropertiesByChapter(long chapterId)
		{
			var chapter = _chapterManager.GetList().Where(t => t.Id == chapterId).SingleOrDefault();

			if (chapter == null)
			{
				throw new Exception($"Не найден раздел с id {chapterId}");
			}

			var listPropertiesIds = new List<long>();

			var parentChapter = chapter;

			while (parentChapter != null)
			{
				var propertiesIds = _chapterPropertiesConnectionManager.GetList().Where(t => t.ChapterId == parentChapter.Id).Select(t => t.PropertiesId).ToArray();
				listPropertiesIds.AddRange(propertiesIds);

				parentChapter = _chapterManager.GetList().Where(t => t.Id == parentChapter.ParentChapterId).SingleOrDefault();
			}

			var properties = _propertiesManager.GetList().Where(t => listPropertiesIds.Contains(t.Id)).ToArray();

			return properties;
		}
	}
}
