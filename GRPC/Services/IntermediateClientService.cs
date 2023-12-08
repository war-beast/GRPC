using Grpc.Core;
using GRPC.Client.Interfaces;

namespace GRPC.Client.Services;

public class IntermediateClientService : IIntermediateClientService
{
	private readonly Intermediate.IntermediateClient _client;

	public IntermediateClientService(Intermediate.IntermediateClient client)
	{
		_client = client;
	}

	public async Task<bool> CallIntermediateMessage(ResendRequest request, string jwt, CancellationToken token)
	{
		try
		{
			var headers = new Metadata
			{
				{ "Authorization", jwt }
			};

			var reply = await _client.ResendMessageAsync(request, headers, deadline: DateTime.UtcNow.Add(TimeSpan.FromSeconds(30)), token);

			return reply.Result;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}