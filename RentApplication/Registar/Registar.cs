using Microsoft.EntityFrameworkCore;
using Rent.DataAccess.Context;

namespace RentApplication.Registar
{
	public static class Registar
	{
		public static void RegisterApi(this IServiceCollection services)
		{
			services.AddScoped<DbContext, RentDbContext>();
		}
	}
}
