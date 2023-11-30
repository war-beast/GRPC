using GRPC.Client.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRPC.Controllers;

[ApiController]
[Route("grpc")]
public class GreeterApiController : ControllerBase
{
	private readonly IGreeterClientService _greeterClientService;

	public GreeterApiController(IGreeterClientService greeterClientService)
	{
		_greeterClientService = greeterClientService;
	}

	[HttpGet("greet/{name}")]
	[Authorize]
	public async Task<IActionResult> CallGreet(string? name, CancellationToken token)
	{
		var auth = Request.Headers["Authorization"];

		return name == null 
			? BadRequest("Сорян, но без имени никак.") 
			: Ok(await _greeterClientService.CallGreeterMessage(name, auth.First(), token));
	}
}