using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using GRPC.Client.Interfaces;
using GRPC.Client.Services;

namespace GRPC;

public static class Initialization
{
	public static void AddCustomServices(this IServiceCollection services)
	{
		services.AddTransient<IGreeterClientService, GreeterClientService>();
		services.AddTransient<IIntermediateClientService, IntermediateClientService>();
		services.AddTransient<IUserService, UserService>();
		services.AddTransient<IStorageClientService, StorageClientService>();

		services.AddScoped<IRepository<AppUser>, UserRepository>();
	}
}