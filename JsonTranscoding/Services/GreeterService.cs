using Grpc.Core;
using JsonTranscoding;

namespace JsonTranscoding.Services
{
	public class GreeterService : Greeter.GreeterBase
	{
		private readonly ILogger<GreeterService> _logger;
		public GreeterService(ILogger<GreeterService> logger)
		{
			_logger = logger;
		}

		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			return Task.FromResult(new HelloReply
			{
				Message = "Hello " + request.Name
			});
		}

		public override Task<GreetResponse> ProcessMessage(GreetRequest request, ServerCallContext context)
		{
			var reversedCategories = request.Categories
				.ToArray()
				.Reverse();

			return Task.FromResult(new GreetResponse
			{
				Text = $"Request from {context.Host}: {request.Name} - {request.Age}",
				Categories = { reversedCategories }
			});
		}
	}
}
