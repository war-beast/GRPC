using DAL;
using GRPC;
using GRPC.Client.Extensions;
using GRPC.Client.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.Configure<Network>(builder.Configuration.GetSection("Docker"));

var connectionString = $"Host={builder.Configuration.GetSection("Docker:Host").Value}{builder.Configuration.GetSection("Database:ConnectionString").Value}";
builder.Services.InitDatabase(connectionString);

builder.Services.AddRpcClients(builder.Configuration);
builder.Services.AddCustomServices();

var app = builder.Build();

app.MigrateDatabase<AppDataContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
