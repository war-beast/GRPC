using GRPC.Client.Interfaces;
using GRPC.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRPC.Controllers;

[ApiController]
[Route("grpc/user")]
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;

	public UsersController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(UserRequest request, CancellationToken token)
	{
		var jwt = await _userService.GetToken(request, token);
		return Ok(jwt);
	}

	[HttpPost("add")]
	public async Task<IActionResult> Add(AddUserRequest request, CancellationToken token)
	{
		var id = await _userService.Add(request, token);
		return Ok(id);
	}
}