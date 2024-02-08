namespace GRPC.Client.Interfaces
{
	public interface IStorageClientService
	{
		Task<string> UploadFile(IFormFile file);

		Task<IReadOnlyCollection<string>> GetFileNames(CancellationToken token);
	}
}
