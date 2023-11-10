using GRPC.Client.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;

namespace GRPC.Client.Services;

public class GreeterClientService : IGreeterClientService
{
	public async Task<string> CallGreeterMessage(string name, CancellationToken token)
	{

		using var channel = GrpcChannel.ForAddress("http://localhost:5112");

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