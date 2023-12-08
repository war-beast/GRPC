using Grpc.Core;
using GRPC.Client.Interfaces;

namespace GRPC.Client.Services;

public class GreeterClientService : IGreeterClientService
{
	private readonly Greeter.GreeterClient _client;

	public GreeterClientService(Greeter.GreeterClient client)
	{
		_client= client;
	}

	public async Task<string> CallGreeterMessage(string name, string jwt, CancellationToken token)
	{
		try
		{
			var headers = new Metadata
			{
				{ "Authorization", jwt }
			};

			var reply = await _client.SayHelloAsync(new HelloRequest { Name = name }, headers, deadline: DateTime.UtcNow.Add(TimeSpan.FromSeconds(30)), token);

			return reply.Message;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}