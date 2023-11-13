using Microsoft.AspNetCore.Server.Kestrel.Core;
using SimpleGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(opt =>
{
	opt.ListenLocalhost(7037, o =>
	{
		o.Protocols = HttpProtocols.Http1AndHttp2;
		o.UseHttps();
	});
});

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.UseGrpcWeb();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>()
	.EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
