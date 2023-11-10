using GRPC.Client.Interfaces;
using GRPC.Client.Services;

namespace GRPC;

public static class Initialization
{
	public static void AddCustomServices(this IServiceCollection services)
	{
		services.AddTransient<IGreeterClientService, GreeterClientService>();
	}
}