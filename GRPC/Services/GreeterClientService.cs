﻿using System.Net;
using System.Security.Authentication;
using System.Transactions;
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