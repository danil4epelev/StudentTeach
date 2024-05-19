using Fx.Ef;
using Microsoft.Extensions.DependencyInjection;
using Rent.DataAccess.Entity;

namespace Rent.DataAccess.Registar
{
	public static class Registar
	{
		public static void RegisterDataAccess(this IServiceCollection services)
		{
			services.AddScoped<IEntityRepository<User>, EntityRepository<User>>();
			services.AddScoped<IEntityRepository<Chapter>, EntityRepository<Chapter>>();
			services.AddScoped<IEntityRepository<RentItem>, EntityRepository<RentItem>>();
			services.AddScoped<IEntityRepository<ChapterPropertiesConnection>, EntityRepository<ChapterPropertiesConnection>>();
			services.AddScoped<IEntityRepository<RentItemPropertiesConnection>, EntityRepository<RentItemPropertiesConnection>>();
			services.AddScoped<IEntityRepository<Properties>, EntityRepository<Properties>>();
		}
	}
}
