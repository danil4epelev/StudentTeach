using Microsoft.Extensions.DependencyInjection;
using Rent.Core.Common.Configuration;
using Rent.Core.Contracts.Helpers;
using Rent.Core.Contracts.Managers;
using Rent.Core.Helpers;
using Rent.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Core.Registar
{
    public static class Registar
	{
		public static void RegisterCore(this IServiceCollection services)
		{
			RegisterManagers(services);
			RegisterHelpers(services);
		}

		private static void RegisterHelpers(IServiceCollection services)
		{
			services.AddScoped<IHasherHelper, HasherHelper>();
			services.AddScoped<IAccountHelper, AccountHelper>();
			services.AddScoped<ICacheHelper, CacheHelper>();
			services.AddScoped<IMailHelper, MailHelper>();
		}

		private static void RegisterManagers(IServiceCollection services)
		{
			services.AddScoped<IUserManager, UserManager>();
			services.AddScoped<IChapterManager, ChapterManager>();
			services.AddScoped<IRentItemManager, RentItemManager>();
			services.AddScoped<IChapterPropertiesConnectionManager, ChapterPropertiesConnectionManager>();
			services.AddScoped<IPropertiesManager, PropertiesManager>();
			services.AddScoped<IRentItemPropertiesConnectionManager, RentItemPropertiesConnectionManager>();
		}

		public static void RegisterSettings(this IServiceCollection services,
					ConnectionStrings connectionStrings,
					MailSettings mailSettings)
		{
			services.AddSingleton(connectionStrings);
			services.AddSingleton(mailSettings);
		}
	}
}
