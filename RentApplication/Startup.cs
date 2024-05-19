using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rent.Core.Common.Configuration;
using Rent.Core.Managers.Profiles;
using Rent.Core.Registar;
using Rent.DataAccess.Context;
using Rent.DataAccess.Registar;
using RentApplication.Common;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RentApplication.Registar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Fx.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Rent.DataAccess.Entity;

namespace RentApplication
{
	public class Startup
	{
		private readonly string[] _corsSettingsOrigins = new string[] { "http://localhost:8080", "http://localhost:51295" };
		private readonly ConnectionStrings _connectionStrings;
		private readonly MailSettings _mailSettings;

		public Startup(
			IConfiguration configuration,
			IWebHostEnvironment env)
		{
			Configuration = configuration;

			var appsettingsProvider = new AppSettingsProvider(env.EnvironmentName);
			_connectionStrings = appsettingsProvider.GetSettings<ConnectionStrings>();
			_mailSettings = appsettingsProvider.GetSettings<MailSettings>();
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddHttpContextAccessor();
			services.AddOData();
			services.AddCors(options => options.AddPolicy("AllowAllCorsPolicy",
							  builder =>
							  {
								  builder.WithOrigins(_corsSettingsOrigins)
								  .AllowAnyHeader()
								  .AllowAnyMethod()
								  .AllowCredentials();
							  })
				);

			services.AddDistributedMemoryCache();
			services.AddSession();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/user/login";
					options.AccessDeniedPath = "/user/accessdenied";
				});

			services.AddAuthorization();

			services.AddDbContext<RentDbContext>(options => options.
				 UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).
				 UseNpgsql(_connectionStrings.RentDatabase));

			services.AddAutoMapper(typeof(AppMappingProfile));
			/*
			services.AddMvc(config =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});*/

			AddServices(services);

			services.AddControllers()
					.AddJsonOptions(options =>
					{
						options.JsonSerializerOptions.PropertyNamingPolicy = null;
					});

			services.AddSignalR();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseExceptionHandler("/error");

			app.UseHttpsRedirection();
			app.UseSession();
			app.UseRouting();

			app.UseCors("AllowAllCorsPolicy");

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.SetTimeZoneInfo(TimeZoneInfo.Utc);
			});
		}

		private void AddServices(IServiceCollection services)
		{
			services.RegisterCore();
			services.RegisterDataAccess();
			services.RegisterApi();
			services.RegisterSettings(_connectionStrings, _mailSettings);
		}
	}

}
