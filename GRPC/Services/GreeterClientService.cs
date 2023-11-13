using System.Net;
using System.Security.Authentication;
using GRPC.Client.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace GRPC.Client.Services;

public class GreeterClientService : IGreeterClientService
{
	private readonly IHttpClientFactory _httpClientFactory;

	public GreeterClientService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task<string> CallGreeterMessage(string name, CancellationToken token)
	{
		//AppContext.SetSwitch("Microsoft.AspNetCore.Server.Kestrel.EnableWindows81Http2", true);
		//HttpClient.DefaultProxy = new WebProxy();
		//var httpClient = _httpClientFactory.CreateClient();
		//httpClient.DefaultRequestVersion = new Version(2, 0);
		//httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

		//var handler = new HttpClientHandler();
		//handler.ServerCertificateCustomValidationCallback =
		//	HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
		var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
		handler.HttpVersion = new Version(1, 1);

		using var channel = GrpcChannel.ForAddress("https://localhost:7037", new GrpcChannelOptions{ HttpHandler = handler});

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