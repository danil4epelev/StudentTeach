using NLog.Web;
using Rent.Core.Logger;
using RentApplication;
using System.Reflection.Metadata;

namespace RentApplication.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			LogProvider.Init();

			try
			{
				LogProvider.LogInfo("Run app");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception exception)
			{
				LogProvider.LogError("Crash app", exception);
				throw;
			}
			finally
			{
				LogProvider.LogInfo("Shutdown app");
				LogProvider.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
		}
	}
}