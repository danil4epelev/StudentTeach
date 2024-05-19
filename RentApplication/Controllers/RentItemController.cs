using Fx.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers;
using Rent.DataAccess.Enum;
using RentApplication.Models;
using Microsoft.AspNet.OData.Query;
using Rent.Core.Managers.Data;
using Rent.Core.Transactions;

namespace RentApplication.Controllers
{
	[ApiController]
	[Route("api/rentitem")]
	public class RentItemController : ControllerBase
	{
		private readonly IRentItemManager _rentItemManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IChapterManager _chapterManager;
		private readonly IChapterHelper _chapterHelper;
		private readonly IPropertiesManager _propertiesManager;
		private readonly IRentItemPropertiesConnectionManager _itemPropertiesConnectionManager;

		public RentItemController(IRentItemManager rentItemManager, IHttpContextAccessor contextAccessor, IChapterManager chapterManager, IChapterHelper chapterHelper, IPropertiesManager propertiesManager, IRentItemPropertiesConnectionManager itemPropertiesConnectionManager)
		{
			_rentItemManager = rentItemManager;
			_contextAccessor = contextAccessor;
			_chapterManager = chapterManager;
			_chapterHelper = chapterHelper;
			_propertiesManager = propertiesManager;
			_itemPropertiesConnectionManager = itemPropertiesConnectionManager;
		}

		[HttpGet]
		[Route("get/{topcount}/{skipcount?}")]
		[Authorize(Roles = "Default, Admin")]
		public async Task<IActionResult> GetAll([FromRoute] int topCount, [FromRoute] int? skipCount, [FromBody] FilterModelBase filterModel)
		{
			var rentIrems = _rentItemManager.GetActiveItems();

			if (skipCount != null)
			{
				if (filterModel.StartFilterElement == null)
				{
					return BadRequest("Не найден стартовый элемент для фильтрации");
				}

				rentIrems = rentIrems
					.Where(t => t.Id <= filterModel.StartFilterElement)
					.OrderByDescending(t => t.Id)
					.Skip(skipCount.Value)
					.Take(topCount);
			}

			else
			{
				rentIrems = rentIrems.OrderByDescending(t => t.Id).Take(topCount);
			}

			var rentItemClientsModels = rentIrems.Select(t => new RentItemClientModel
			{
				Id = t.Id,
				Name = t.Name,
				Description = t.Description,
				Price = t.Price,
				PriceType = t.PriceType,
			}).ToArray();


			return Ok(new PageModel<RentItemClientModel>
			{
				Data = rentItemClientsModels,
				Total = rentItemClientsModels.Length
			});
		}

		[HttpPost]
		[Route("create")]
		[Authorize(Roles = "Default, Admin")]
		public async Task<IActionResult> CreateItem([FromBody] RentItemCreateModel data)
		{
			var userId = long.Parse(_contextAccessor.HttpContext?.User.Claims.Where(t => t.Type == CustomClaimsTypes.Id).FirstOrDefault()?.Value);

			if (data == null)
			{
				return NotFound("Не найдена модель данных");
			}

			if (!PriceTypeEnumChunk.GetList().Where(t => t.Value == data.PriceType).Any())
			{
				return NotFound("Не найден тип цены");
			}

			var chapter = _chapterManager.GetList().Where(t => t.Id == data.ChapterId).SingleOrDefault();

			if (chapter == null)
			{
				return NotFound("Не найден раздел");
			}

			if (_chapterManager.GetList().Where(t => t.ParentChapterId == chapter.Id).Any())
			{
				return BadRequest("Раздел должен быть конечным в дереве");
			}

			if (data.Price <= 0)
			{
				return BadRequest("Цена не может быть меньше или равна 0");
			}


			try
			{
				var propertiesChapter = _chapterHelper.GetAllTreePropertiesByChapter(chapter.Id);

				if (propertiesChapter == null)
				{
					return NotFound("Не найдены свойства родительских разделов");
				}

				var rentItemsPropertiesIds = data.Propertes.Select(t => t.Id).ToArray();
				var requiredPropertiesIds = propertiesChapter.Where(t => t.IsRequired).Select(t => t.Id).ToArray();

				if (!requiredPropertiesIds.Where(t => rentItemsPropertiesIds.Contains(t)).Any())
				{
					return BadRequest("Все обязательные поля должны быть заполнены");
				}

				foreach (var property in data.Propertes)
				{
					if (!_propertiesManager.GetList().Where(t => t.Id == property.Id).Any())
					{
						return BadRequest($"Не найдено поле с id {property.Id}");
					}

					if (!propertiesChapter.Where(t => t.Id == property.Id).Any())
					{
						return BadRequest($"Свойство с id {property.Id} не привязано к дереву разделов");
					}
				}

				using (var scope = new RequiredTransactionScope())
				{
					var rentItemId = await _rentItemManager.AddAsync(new Rent.Core.Managers.Data.RentItemData
					{
						AuthorId = userId,
						ChapterId = chapter.Id,
						Description = data.Description,
						DtCreate = DateTime.Now.ToUniversalTime(),
						Status = (int)RentItemStatusEnum.Draft,
						Name = data.Name,
						Price = data.Price,
						PriceType = data.PriceType
					});

					foreach (var property in data.Propertes)
					{
						
					}

					return Ok();
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("sendmoderation/{id}")]
		[Authorize(Roles = "Default, Admin")]
		public async Task<IActionResult> SendToModeration([FromRoute] long itemId)
		{
			var item = _rentItemManager.GetList().Where(t => t.Id == itemId).SingleOrDefault();

			if (item == null)
			{
				return NotFound("Не найден элемент аренды");
			}

			if (item.Status != (int)RentItemStatusEnum.Draft || item.Status != (int)RentItemStatusEnum.Rejected)
			{
				return BadRequest("Элемент аренды имеет неподходящий статус");
			}

			item.Status = (int)RentItemStatusEnum.Moderated;
			item.DtSendToModeration = DateTime.Now.ToUniversalTime();

			await _rentItemManager.UpdateAsync(itemId, item);

			return Ok();
		}

		[HttpGet]
		[Route("get/onmoderation")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetItemsOnModeration([FromBody] RentItemCreateModel data)
		{
			var items = _rentItemManager.GetList().Where(t => t.Status == (int)RentItemStatusEnum.Moderated).ToArray();
			return Ok(items);
		}

		[HttpPost]
		[Route("approve/moderate/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ApproveItem([FromRoute] long id)
		{
			var item = _rentItemManager.GetList().Where(t => t.Id == id).SingleOrDefault();

			if (item == null)
			{
				return NotFound("Не найден элемент аренды");
			}

			if (item.Status != (int)RentItemStatusEnum.Moderated)
			{
				return BadRequest("Элемент аренды имеет неподходящий статус");
			}

			item.Status = (int)RentItemStatusEnum.Active;
			item.DtApprove = DateTime.Now.ToUniversalTime();

			await _rentItemManager.UpdateAsync(id, item);

			return Ok();
		}

		[HttpPost]
		[Route("reject/moderate/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> RejectItem([FromRoute] long id, [FromBody] RejectModel rejectModel)
		{
			if (rejectModel == null || string.IsNullOrEmpty(rejectModel.RejectReason))
			{
				return BadRequest("Отсутсвует причина отказа");
			}

			var item = _rentItemManager.GetList().Where(t => t.Id == id).SingleOrDefault();

			if (item == null)
			{
				return NotFound("Не найден элемент аренды");
			}

			if (item.Status != (int)RentItemStatusEnum.Moderated)
			{
				return BadRequest("Элемент аренды имеет неподходящий статус");
			}

			item.Status = (int)RentItemStatusEnum.Rejected;
			item.RejectedRemarks = rejectModel.RejectReason;
			item.DtReject = DateTime.Now.ToUniversalTime();

			await _rentItemManager.UpdateAsync(id, item);

			return Ok();
		}


	}
}
