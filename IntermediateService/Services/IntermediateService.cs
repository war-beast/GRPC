using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace IntermediateService.Services
{
	[Authorize]
	public class IntermediateService : Intermediate.IntermediateBase
	{
		private readonly ILogger<IntermediateService> _logger;
		private readonly Greeter.GreeterClient _client;

		public IntermediateService(ILogger<IntermediateService> logger, 
			Greeter.GreeterClient client)
		{
			_logger = logger;
			_client = client;
		}

		public override async Task<ResendReply> ResendMessage(ResendRequest request, ServerCallContext context)
		{
			try
			{
				var headers = context.RequestHeaders;

				var reply = await _client.SayHelloAsync(new HelloRequest { Name = request.Name}, headers, deadline: context.Deadline, context.CancellationToken);

				_logger.LogInformation("Получен ответ: {reply}", reply.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				return new ResendReply { Result = false };
			}

			return new ResendReply
			{
				Result = true
			};
		}
	}
}
