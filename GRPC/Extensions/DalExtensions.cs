using DAL;
using Microsoft.EntityFrameworkCore;

namespace GRPC.Client.Extensions;

public static class DalExtensions
{
	public static void InitDatabase(this IServiceCollection services, string? connectionString)
	{
		services.AddDbContext<AppDataContext>(options =>
			{
				options.UseNpgsql(connectionString, builder => builder.CommandTimeout(90)).EnableSensitiveDataLogging();
			});
	}

	public static WebApplication MigrateDatabase<T>(this WebApplication webHost) where T : DbContext
	{
		using var scope = webHost.Services.CreateScope();
		var services = scope.ServiceProvider;
		try
		{
			var db = services.GetRequiredService<T>();
			db.Database.Migrate();
		}
		catch (Exception ex)
		{
			var logger = services.GetRequiredService<ILogger<Program>>();
			logger.LogError(ex, "An error occurred while migrating the database.");
		}

		return webHost;
	}

}