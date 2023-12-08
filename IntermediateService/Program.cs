using IntermediateService;
using IntermediateService.Models;
using IntermediateService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.Configure<Network>(builder.Configuration.GetSection("Docker"));

builder.Services.AddRpcClients(builder.Configuration);
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseGrpcWeb();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<IntermediateService.Services.IntermediateService>()
	.EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
