using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Rent.Core.Managers.Data;
using Rent.DataAccess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Helpers
{
    public class AccountHelper : IAccountHelper
	{
		private readonly IHasherHelper _hasherHelper;
		private readonly IUserManager _userManager;
		public AccountHelper(IHasherHelper hasherHelper, IUserManager userManager)
		{ 
			_hasherHelper = hasherHelper;
			_userManager = userManager;
		}

		public async Task RegisterAccount(string email, string password)
		{
			await _userManager.AddAsync(new UserData
			{
				Email = email,
				PasswordHash = _hasherHelper.HashPassword(password),
				Login = email,
				RoleType = (int)RolesEnum.Default
			});

		}
	}
}
