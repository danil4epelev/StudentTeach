using Microsoft.AspNetCore.Mvc;
using RentApplication.Models;
using System.Security.Claims;
using Rent.DataAccess.Enum;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;
using Rent.Core.Models;
using Rent.Core.Exceptions;
using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Fx.Auth;

namespace RentApplication.Controllers
{
    [ApiController]
	[Route("api/account")]
	public class UserController : ControllerBase
	{
		private readonly IAccountHelper _accountHelper;
		private readonly IUserManager _userManager;
		private readonly IHasherHelper _hasherHelper;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ICacheHelper _cacheHelper;
		private readonly IMailHelper _mailHelper;
		public UserController(IAccountHelper accountHelper, 
			IUserManager userManager,
			IHasherHelper hasherHelper,
			IHttpContextAccessor contextAccessor,
			ICacheHelper cacheHelper,
			IMailHelper mailHelper)
		{
			_accountHelper = accountHelper;
			_userManager = userManager;
			_hasherHelper = hasherHelper;
			_contextAccessor = contextAccessor;
			_cacheHelper = cacheHelper;
			_mailHelper = mailHelper;
		}

		[HttpPost]
		[Route("register/approove")]
		public async Task<IActionResult> RegisterApprove([FromBody] ApprooveRegisterModel modelRegister)
		{
			if (modelRegister == null)
			{
				return BadRequest("Отсутствует модель данных для запроса");
			}

			var model = _cacheHelper.GetApproveModel(modelRegister.ModelId);

			if (model == null)
			{
				return BadRequest("Не найдена модель данных. Вероятно, истёк срок действия кода.");
			}

			if (model.ApproveCode != modelRegister.ApproveCode)
			{
				return BadRequest("Код для подтверждения неверный.");
			}

			await _accountHelper.RegisterAccount(model.Email, model.Password);

			return Ok();
		}

		[HttpPost]
		[Route("register/validate")]
		public async Task<IActionResult> RegisterValidate([FromBody] RegisterUserModels model)
		{
			var errorList = new List<string>();

			if (_userManager.GetList().Where(t => t.Email ==  model.Email).Any())
			{
				return BadRequest("Пользователь с такой почтой уже зарегестрирован.");
			}

			if (!ValidatePassword(model.Password, errorList))
			{
				return BadRequest(errorList);
			}

			if (!ValidateEmail(model.Email))
			{
				return BadRequest("Проверьте правильность заполнения поля Email");
			}

			var rnd = new Random();
			var code = rnd.Next(100000, 999999);

			var modelId = _cacheHelper.SetApproveModel(new RegisterUserModelsExpanded
			{
				ApproveCode = code,
				Email = model.Email,
				Password = model.Password
			});

			await _mailHelper.SendMessageAsync(
				"Подтверждение почты",
				$"Код для подтверждения почты: {code}. Код действителен в течении 10 минут.",
				new List<string> { model.Email });

			return Ok(modelId);
		}

		[HttpPost]
		[Route("recovery")]
		public async Task<IActionResult> Recovery([FromBody] BaseUserModel model)
		{
			var errorList = new List<string>();

			if (!_userManager.GetList().Where(t => t.Email == model.Email).Any())
			{
				return BadRequest("Пользователь с такой почтой не зарегестрирован.");
			}

			var rnd = new Random();
			var code = rnd.Next(100000, 999999);

			var modelId = _cacheHelper.SetApproveModel(new RegisterUserModelsExpanded
			{
				ApproveCode = code,
				Email = model.Email
			});

			await _mailHelper.SendMessageAsync(
				"Восстановление аккаунта",
				$"Код для восстановления вашего аккаунта: {code}. Код действителен в течении 10 минут.",
				new List<string> { model.Email });

			return Ok(modelId);
		}

		[HttpPost]
		[Route("recovery/approove")]
		public async Task<IActionResult> ApproveRecovery([FromBody] ApprooveRegisterModel modelRegister)
		{
			if (modelRegister == null)
			{
				return BadRequest("Отсутствует модель данных для запроса");
			}

			var model = _cacheHelper.GetApproveModel(modelRegister.ModelId);

			if (model == null)
			{
				return BadRequest("Не найдена модель данных. Вероятно, истёк срок действия кода.");
			}

			if (model.ApproveCode != modelRegister.ApproveCode)
			{
				return BadRequest("Код для подтверждения неверный.");
			}

			return Ok();
		}

		[HttpPost]
		[Route("resetpassword")]
		public async Task<IActionResult> RessetPassword([FromBody] ApprooveResetPasswordModel modelRegister)
		{
			if (modelRegister == null)
			{
				return BadRequest("Отсутствует модель данных для запроса");
			}

			var model = _cacheHelper.GetApproveModel(modelRegister.ModelId);

			if (model == null)
			{
				return BadRequest("Не найдена модель данных. Вероятно, истёк срок действия кода.");
			}

			if (model.ApproveCode != modelRegister.ApproveCode)
			{
				return BadRequest("Код для подтверждения неверный.");
			}

			var errorList = new List<string>();
			if (!ValidatePassword(modelRegister.Password, errorList))
			{
				return BadRequest(errorList);
			}

			var user = _userManager.GetList().Where(t => t.Email == model.Email).SingleOrDefault();
			if (user == null)
			{
				return BadRequest("Пользователь не найден");
			}

			user.PasswordHash = _hasherHelper.HashPassword(modelRegister.Password);

			await _userManager.UpdateAsync(user.Id, user);

			return Ok();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IResult> Login([FromBody] LoginUserModels model)
		{
			if (model == null)
				Results.Unauthorized();

			var user = _userManager.GetList().Where(t => t.Login == model.Login || t.Email == model.Login).SingleOrDefault();

			if (user == null)
				Results.Unauthorized();

			if (!_hasherHelper.VerifyHashedPassword(user.PasswordHash, model.Password))
				Results.Unauthorized();

			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, RolesChunk.GetList().Where(t => t.Value == user.RoleType).SingleOrDefault()?.Name),
				new Claim(CustomClaimsTypes.Id, user.Id.ToString())
			};

			var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			await _contextAccessor.HttpContext?.SignInAsync(claimsPrincipal);
			
			return Results.Ok();
		}

		[HttpGet]
		[Route("accessdenied")]
		public async Task<IActionResult> Accessdenied()
		{
			return Forbid();
		}

		private bool ValidateEmail(string email)
		{
			string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
			Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
			return isMatch.Success;
		}

		private bool ValidatePassword(string password, List<string> errors)
		{

			// Проверка на длину пароля
			if (password.Length < 8)
			{
				errors.Add("Минимальная длинна пароля 8 символов!");
			}

			// Проверка на наличие хотя бы одной цифры
			if (!password.Any(char.IsDigit))
			{
				errors.Add("Пароль должен содержать хотя бы одну цифру!");
			}

			// Проверка на наличие хотя бы одной строчной буквы
			if (!password.Any(char.IsLower))
			{
				errors.Add("Пароль должен содержать хотя бы одну прописную букву!");
			}

			// Проверка на наличие хотя бы одной заглавной буквы
			if (!password.Any(char.IsUpper))
			{
				errors.Add("Пароль должен содержать хотя бы одну заглавную букву!");
			}

			// Все проверки пройдены, пароль действителен
			return !errors.Any();
		}
	}
}
