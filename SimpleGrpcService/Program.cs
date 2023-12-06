using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;
using SimpleGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(opt => opt.EnableDetailedErrors = true);
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			// ��������, ����� �� �������������� �������� ��� ��������� ������
			ValidateIssuer = true,
			// ������, �������������� ��������
			ValidIssuer = AuthOptions.ISSUER,

			// ����� �� �������������� ����������� ������
			ValidateAudience = true,
			// ��������� ����������� ������
			ValidAudience = AuthOptions.AUDIENCE,
			// ����� �� �������������� ����� �������������
			ValidateLifetime = true,

			// ��������� ����� ������������
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			// ��������� ����� ������������
			ValidateIssuerSigningKey = true
		};
		options.SaveToken = true;
	});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseGrpcWeb();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>()
	.EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
