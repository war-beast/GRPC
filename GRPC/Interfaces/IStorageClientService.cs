namespace GRPC.Client.Interfaces
{
	public interface IStorageClientService
	{
		Task UploadFile(IFormFile file);
	}
}
