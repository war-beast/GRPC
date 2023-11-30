using System.Security.Claims;
using System.Security.Cryptography;
using GRPC.Client.Interfaces;
using GRPC.Client.Models;
using System.Text;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Shared;

namespace GRPC.Client.Services;

public class UserService : IUserService
{
	private readonly IRepository<AppUser> _userRepository;

	public UserService(IRepository<AppUser> userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<string> GetToken(UserRequest request, CancellationToken token)
	{
		try
		{
			var hasher = GetPasswordHash;
			var user = await _userRepository.Where(x => request.Email.Equals(x.Email) && hasher(request.Password).Equals(x.PasswordHash))
				.FirstAsync(token);

			var identity = GetIdentity(user);

			var now = DateTime.UtcNow;
			JwtSecurityToken jwt = new(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				notBefore: now,
				claims: identity.Claims,
				expires: now.AddDays(AuthOptions.LIFETIME),
				signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
			return encodedJwt;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public Task<Guid> Add(AddUserRequest request, CancellationToken token)
	{
		var user = new AppUser
		(
			Guid.NewGuid(),
			request.Name,
			request.Email,
			GetPasswordHash(request.Password)
		);

		return _userRepository.Create(user, token);
	}

	private static string GetPasswordHash(string password) =>
		Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

	private ClaimsIdentity GetIdentity(AppUser user)
	{
		var claims = new List<Claim>
		{
			new(ClaimsIdentity.DefaultNameClaimType, user.Name),
			new("email", user.Email),
			new("userId", user.Id.ToString())
		};

		return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, string.Empty);
	}
}