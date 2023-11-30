using Grpc.Core;
using Grpc.Net.Client;
using GRPC.Client.Interfaces;
using GRPC.Client.Models;
using Microsoft.Extensions.Options;

namespace GRPC.Client.Services;

public class GreeterClientService : IGreeterClientService
{
	private readonly Network _network;

	public GreeterClientService(IOptions<Network> networkSnapshot)
	{
		_network = networkSnapshot.Value;
	}

	public async Task<string> CallGreeterMessage(string name, string jwt, CancellationToken token)
	{
		var handler = new HttpClientHandler();
		handler.ServerCertificateCustomValidationCallback =
			HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

		using var channel = GrpcChannel.ForAddress($"https://{_network.Host}:7037", new GrpcChannelOptions { HttpHandler = handler });

		try
		{
			var client = new Greeter.GreeterClient(channel);
			var headers = new Metadata
			{
				{ "Authorization", jwt }
			};

			var reply = await client.SayHelloAsync(new HelloRequest { Name = name }, headers, DateTime.MaxValue, token);

			return reply.Message;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}