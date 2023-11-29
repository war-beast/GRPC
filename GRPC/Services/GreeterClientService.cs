using Grpc.Net.Client;
using GRPC.Client.Interfaces;

namespace GRPC.Client.Services;

public class GreeterClientService : IGreeterClientService
{
	public async Task<string> CallGreeterMessage(string name, CancellationToken token)
	{
		var handler = new HttpClientHandler();
		handler.ServerCertificateCustomValidationCallback =
			HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

		using var channel = GrpcChannel.ForAddress("https://host.docker.internal:7037", new GrpcChannelOptions { HttpHandler = handler });

		try
		{
			var client = new Greeter.GreeterClient(channel);

			var reply = await client.SayHelloAsync(new HelloRequest { Name = name }, deadline: DateTime.MaxValue, cancellationToken: token);

			return reply.Message;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}