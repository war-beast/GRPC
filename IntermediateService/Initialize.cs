using Intermediate.DAL.Context;
using Intermediate.DAL.Interfaces;
using Intermediate.DAL.Repositories;

namespace IntermediateService;

public static class Initialize
{
	public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<DatabaseOptions>(
			configuration.GetSection("Database")
		);

		services.AddTransient<IRequestRepository, RequestRepository>();

		services.AddSingleton<IDbContext, DbContext>();
	}
}