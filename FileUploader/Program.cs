using FileUploader.Services;
using Minio;
using Minio.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddMinio("grpc", options =>
{
	options.Endpoint = "host.docker.internal:9000";
	options.AccessKey = "h2mvsldpgXfpcqVN5bQX";
	options.SecretKey = "W7plH3UuoncU7kcFR8cnZ92SUoyf2uc9svFPpgt3";
	
	
	options.ConfigureClient(client =>
	{
		client.WithSSL(false);
	});
});

var app = builder.Build();

app.UseGrpcWeb();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<UploaderService>()
	.EnableGrpcWeb();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
