using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers.Data;
using Rent.Core.Models;
using Rent.Core.Transactions;
using Rent.DataAccess.Entity;
using Rent.DataAccess.Enum;
using RentApplication.Models;
using System.Text.Json;

namespace RentApplication.Controllers
{
	[ApiController]
	[Route("api/chapter")]
#if !DEBUG
	[Authorize(Roles = "Admin")]
#endif
	public class ChapterController : ControllerBase
	{
		private readonly IChapterManager _chapterManager;
		private readonly IChapterHelper _chapterHelper;
		private readonly IPropertiesManager _propertiesManager;
		private readonly IChapterPropertiesConnectionManager _chapterPropertiesConnectionManager;
		private readonly IRentItemPropertiesConnectionManager _rentItemPropertiesConnectionManager;

		public ChapterController(IChapterManager chapterManager,
			IChapterHelper chapterHelper,
			IPropertiesManager propertiesManager,
			IChapterPropertiesConnectionManager chapterPropertiesConnectionManager,
			IRentItemPropertiesConnectionManager rentItemPropertiesConnectionManager)
		{
			_chapterManager = chapterManager;
			_chapterHelper = chapterHelper;
			_propertiesManager = propertiesManager;
			_chapterPropertiesConnectionManager = chapterPropertiesConnectionManager;
			_rentItemPropertiesConnectionManager = rentItemPropertiesConnectionManager;
		}

		[HttpPost]
		[Route("off/properties")]
		public async Task<IActionResult> OffFromChapterProperties([FromBody] PropertiesConnectionModel addModel)
		{
			if (addModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			if (!_chapterManager.GetList().Where(t => t.Id == addModel.ChapterId).Any())
			{
				return NotFound($"Не найден раздел с id {addModel.ChapterId}");
			}

			if (addModel.PropertiesIds != null && addModel.PropertiesIds.Any())
			{
				foreach (var id in addModel.PropertiesIds)
				{
					var property = _propertiesManager.GetList().Where(t => t.Id == id).SingleOrDefault();
					if (property == null)
					{
						return NotFound($"Свойство с id {id} не найдено");
					}

					var connection = _chapterPropertiesConnectionManager.GetList().Where(t => t.PropertiesId == id && addModel.ChapterId == t.ChapterId).SingleOrDefault();
					if (connection != null)
					{
						await _chapterPropertiesConnectionManager.DeleteAsync(connection.Id);
					}
				}
			}

			return Ok();
		}

		[HttpPost]
		[Route("add/properties")]
		public async Task<IActionResult> AddToChapterProperties([FromBody] PropertiesConnectionModel addModel)
		{
			if (addModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			if (!_chapterManager.GetList().Where(t => t.Id == addModel.ChapterId).Any())
			{
				return NotFound($"Не найден раздел с id {addModel.ChapterId}");
			}

			if (addModel.PropertiesIds != null && addModel.PropertiesIds.Any())
			{
				foreach (var id in addModel.PropertiesIds)
				{
					var property = _propertiesManager.GetList().Where(t => t.Id == id).SingleOrDefault();
					if (property == null)
					{
						return NotFound($"Свойство с id {id} не найдено");
					}

					if (_chapterPropertiesConnectionManager.GetList().Where(t => t.PropertiesId == id && addModel.ChapterId == t.ChapterId).Any())
					{
						return BadRequest($"Уже существует связь между разделом с id {addModel.ChapterId} и свойством с id {id}");
					}

					await _chapterPropertiesConnectionManager.AddAsync(new ChapterPropertiesConnectionData
					{
						ChapterId = addModel.ChapterId,
						PropertiesId = id
					});
				}
			}

			return Ok();
		}

		[HttpPost]
		[Route("update/properties")]
		public async Task<IActionResult> UpdateChapterProperties([FromBody] UpdatePropertyModelcs propertyModel)
		{
			if (propertyModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			var property = _propertiesManager.GetList().Where(t => t.Id == propertyModel.Id).SingleOrDefault();
			if (property == null)
			{
				return NotFound($"Свойство с id {propertyModel.Id} не найдено");
			}

			property.Name = propertyModel.Name;
			property.IsRequired = propertyModel.IsRequired;
			await _propertiesManager.UpdateAsync(property.Id, property);

			return Ok();
		}

		[HttpDelete]
		[Route("delete/property/{id}")]
		public async Task<IActionResult> DeleteProperties([FromRoute] long id)
		{
			var deleted = _propertiesManager.GetList().Where(t => t.Id == id).SingleOrDefault();
			if (deleted == null)
			{
				return NotFound($"Не найдено свойство с id {id}");
			}

			using (var scope = new RequiredTransactionScope())
			{
				var chapterConnections = _chapterPropertiesConnectionManager.GetList().Where(t => t.PropertiesId == id).ToArray();
				foreach (var chapterConnection in chapterConnections)
				{
					await _chapterPropertiesConnectionManager.DeleteAsync(chapterConnection.Id);
				}

				var rentItemConnections = _rentItemPropertiesConnectionManager.GetList().Where(t => t.PropertiesId == id).ToArray();
				foreach (var rentItemConnection in rentItemConnections)
				{
					await _rentItemPropertiesConnectionManager.DeleteAsync(rentItemConnection.Id);
				}

				await _propertiesManager.DeleteAsync(id);
			}

			return Ok();
		}


		[HttpGet]
		[Route("get/properties")]
		public async Task<IActionResult> GetAllProperties()
		{
			return Ok(_propertiesManager.GetList().ToArray());
		}

		[HttpPost]
		[Route("create/properties")]
		public async Task<IActionResult> CreateChapterProperties([FromBody] Rent.Core.Models.PropertyModel propertyModel)
		{
			if (propertyModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			if (string.IsNullOrEmpty(propertyModel.Name))
			{
				return BadRequest("Наименование не должно быть пустым.");
			}

			if (_propertiesManager.GetList().Where(t => t.Name == propertyModel.Name).Any())
			{
				return BadRequest("Свойство с таким наименованием уже существует.");
			}

			if (!TypePropertiesChunk.GetList().Where(t => t.Value == propertyModel.TypeProperties).Any())
			{
				return NotFound($"Свойства с типом {propertyModel.TypeProperties} не найдено");
			}

			string values = null;

			if (propertyModel.Values != null && propertyModel.Values.Any())
			{
				if (propertyModel.TypeProperties != (int)TypeProperties.Enum)
				{
					return BadRequest("Для того, чтобы задать значения для перечисления тип свойства должен быть enum");
				}

				values = string.Join(";", propertyModel.Values);
			}

			var propertyId = await _propertiesManager.AddAsync(new PropertiesData
			{
				IsRequired = propertyModel.IsRequired,
				Name = propertyModel.Name,
				TypeProperties = propertyModel.TypeProperties,
				Values = values
			});

			return Ok();
		}


		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateChapter([FromBody] CreateChapterModel createModel)
		{
			if (createModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			if (_chapterManager.GetList().Where(t => t.Name == createModel.Name).Any())
			{
				return BadRequest("Раздел с таким наименование уже существует.");
			}

			if (string.IsNullOrEmpty(createModel.Name))
			{
				return BadRequest("Наименование не должно быть пустым.");
			}

			if (createModel.ParentId != null && !_chapterManager.GetList().Where(t => t.Id == createModel.ParentId).Any())
			{
				return BadRequest($"Родительский раздел и id {createModel.ParentId} не найден.");
			}

			var chapterId = await _chapterManager.AddAsync(new ChapterData
			{
				Name = createModel.Name,
				ParentChapterId = createModel.ParentId,
			});

			if (createModel.PropertiesIds != null && createModel.PropertiesIds.Any())
			{
				foreach (var id in createModel.PropertiesIds)
				{
					var property = _propertiesManager.GetList().Where(t => t.Id == id).SingleOrDefault();
					if (property == null)
					{
						return NotFound($"Свойство с id {id} не найдено");
					}

					await _chapterPropertiesConnectionManager.AddAsync(new ChapterPropertiesConnectionData
					{
						ChapterId = chapterId,
						PropertiesId = id
					});
				}
			}

			return Ok(chapterId);
		}

		[HttpPost]
		[Route("update")]
		public async Task<IActionResult> UpdateChapter([FromBody] UpdateChapterModel updateModel)
		{
			if (updateModel == null)
			{
				return BadRequest("Отсутствует модель данных.");
			}

			var chapter = _chapterManager.GetList().Where(t => t.Id == updateModel.Id).SingleOrDefault();
			if (chapter == null)
			{
				return NotFound($"Не найден раздел с id {updateModel.Id}");
			}

			if (_chapterManager.GetList().Where(t => t.Name == updateModel.Name).Any())
			{
				return BadRequest("Раздел с таким наименование уже существует.");
			}

			if (string.IsNullOrEmpty(updateModel.Name))
			{
				return BadRequest("Наименование не должно быть пустым.");
			}

			if (updateModel.ParentId != null && !_chapterManager.GetList().Where(t => t.Id == updateModel.ParentId).Any())
			{
				return BadRequest($"Родительский раздел и id {updateModel.ParentId} не найден.");
			}

			chapter.ParentChapterId = updateModel.ParentId;
			chapter.Name = updateModel.Name;

			await _chapterManager.UpdateAsync(updateModel.Id, chapter);

			return Ok(chapter);
		}

		[HttpDelete]
		[Route("delete/{chapterid}")]
		public async Task<IActionResult> DeleteChapter([FromRoute] long chapterId)
		{
			if (!_chapterManager.GetList().Where(t => t.Id == chapterId).Any())
			{
				return NotFound();
			}

			var allChilds = GetAllChildsChapter(chapterId).OrderByDescending(t => t.ParentChapterId);

			foreach (var chapter in allChilds)
			{
				await _chapterManager.DeleteAsync(chapter.Id);
			}

			return Ok();
		}

		[HttpGet]
		[Route("get/properties/{chapterid}")]
		public async Task<IActionResult> GetAllPropertiesByChapter(long chapterId)
		{
			var chapter = _chapterManager.GetList().Where(t => t.Id == chapterId).SingleOrDefault();

			if (chapter == null)
			{
				return BadRequest($"Раздел с id {chapterId} не найден.");
			}

			try
			{
				var properties = _chapterHelper.GetAllTreePropertiesByChapter(chapterId);

				return Ok(properties);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("get/main")]
		public async Task<IActionResult> GetMainChapters()
		{
			return Ok(_chapterManager.GetList().Where(t => t.ParentChapterId == null));
		}

		[HttpGet]
		[Route("get/childs/{chapterid}")]
		public async Task<IActionResult> GetChildsChapter([FromRoute] long chapterId)
		{
			if (!_chapterManager.GetList().Where(t => t.Id == chapterId).Any())
			{
				return NotFound();
			}

			return Ok(_chapterManager.GetList().Where(t => t.ParentChapterId == chapterId));
		}

		/// <summary>
		/// Получить все дочернии разделы, включая сам родительский
		/// </summary>
		/// <param name="parentId">ИД родительского раздела</param>
		/// <returns>Лист разделов</returns>
		private List<ChapterData> GetAllChildsChapter(long parentId)
		{
			var childs = _chapterManager.GetList().Where(t => t.ParentChapterId == parentId).ToList();
			var parent = _chapterManager.GetList().Where(t => t.Id == parentId).SingleOrDefault();
			var allChilds = new List<ChapterData> { parent };

			foreach (var child in childs)
			{
				allChilds.AddRange(GetAllChildsChapter(child.Id));
			}

			return allChilds;
		}
	}
}
