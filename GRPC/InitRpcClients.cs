using GRPC.Client;

namespace GRPC;

public static class InitRpcClients
{
	public static void AddRpcClients(this IServiceCollection services, IConfiguration config)
	{
		var host = config.GetSection("Docker:Host").Value;

		services.AddGrpcClient<Greeter.GreeterClient>("Greeter", o =>
		{
			o.Address = new Uri($"https://{host}:{config.GetSection("RpcServicesPorts:Greeting").Value}");
		})
		.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback =
				HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
		}); 
		
		services.AddGrpcClient<Intermediate.IntermediateClient>("Intermediate", o =>
			{
				o.Address = new Uri($"https://{host}:{config.GetSection("RpcServicesPorts:Intermediate").Value}");
			})
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
					HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});

		services.AddGrpcClient<FileOP.FileOPClient>("FileStorage", opt =>
		{
			opt.Address = new Uri($"https://{host}:{config.GetSection("RpcServicesPorts:FileStorage").Value}");
		})
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback =
					HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			});
	}
}