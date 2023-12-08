using Grpc.Core;
using Intermediate.DAL.Entities;
using Intermediate.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace IntermediateService.Services
{
	[Authorize]
	public class IntermediateService : Intermediate.IntermediateBase
	{
		private readonly ILogger<IntermediateService> _logger;
		private readonly Greeter.GreeterClient _client;
		private readonly IRequestRepository _requestRepository;
		private readonly IHttpContextAccessor _contextAccessor;

		public IntermediateService(ILogger<IntermediateService> logger, 
			Greeter.GreeterClient client, 
			IRequestRepository requestRepository, 
			IHttpContextAccessor contextAccessor)
		{
			_logger = logger;
			_client = client;
			_requestRepository = requestRepository;
			_contextAccessor = contextAccessor;
		}

		public override async Task<ResendReply> ResendMessage(ResendRequest request, ServerCallContext context)
		{
			try
			{
				var headers = context.RequestHeaders;

				var reply = await _client.SayHelloAsync(new HelloRequest { Name = request.Name}, headers, deadline: context.Deadline, context.CancellationToken);

				_logger.LogInformation("Получен ответ: {reply}", reply.Message);

				var contextRequest = new Request
				{
					AppUserName = _contextAccessor.HttpContext?.User.Identity?.Name ?? "Unauthenticated user",
					CallDateTime = DateTime.UtcNow,
					RequestDigits = request.Digits,
					RequestInteger = request.NullableInt,
					RequestName = request.Name
				};

				await _requestRepository.SaveRequest(contextRequest);
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
