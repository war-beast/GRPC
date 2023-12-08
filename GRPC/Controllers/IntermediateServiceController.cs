using GRPC.Client;
using GRPC.Client.Interfaces;
using GRPC.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRPC.Controllers;

[ApiController]
[Route("grpc")]
public class IntermediateServiceController : ControllerBase
{
	private readonly IIntermediateClientService _clientService;

	public IntermediateServiceController(IIntermediateClientService clientService)
	{
		_clientService = clientService;
	}

	[HttpPost("intermediate")]
	[Authorize]
	public async Task<IActionResult> CallIntermediate(InterRequest request, CancellationToken token)
	{
		var auth = Request.Headers["Authorization"];

		var result = await _clientService.CallIntermediateMessage(new ResendRequest
		{
			Digits = { request.Digits },
			Name = request.Name,
			NullableInt = request.NullableInt
		}, auth.First(), token);

		return result
			? Ok()
			: BadRequest("Ашипка, братан");
	}
}