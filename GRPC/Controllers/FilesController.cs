using GRPC.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GRPC.Client.Controllers;

[Route("grpc")]
[ApiController]
public class FilesController : ControllerBase
{
	private readonly IStorageClientService _storageClientService;

	public FilesController(IStorageClientService storageClientService)
	{
		_storageClientService = storageClientService;
	}

	[HttpPost("upload")]
	public async Task<IActionResult> Upload(IFormCollection formData)
	{
		var files = formData.Files;
		var result = await _storageClientService.UploadFile(files[0]);

		return result.Equals(string.Empty, StringComparison.InvariantCultureIgnoreCase) 
		? Ok(result)
		: BadRequest(result);
	}
}
