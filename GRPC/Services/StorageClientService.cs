using Google.Protobuf;
using Grpc.Net.Client;
using GRPC.Client.Interfaces;
using static GRPC.Client.FileOP;

namespace GRPC.Client.Services
{
	public class StorageClientService : IStorageClientService
	{
		private const string Appcode = "grpcClient";
		private readonly FileOPClient _client;

		public StorageClientService(FileOPClient client)
		{
			_client = client;
		}

		public async Task<string> UploadFile(IFormFile file)
		{
			using var stream = file.OpenReadStream();
			var totallength = file.Length;
			var buffer = new byte[1024 * 1024];

			var request = _client.UploadFile();

			FileUploadStatus fileOpResult;
			try
			{
				while (totallength > 0)
				{
					var len = await stream.ReadAsync(buffer);
					totallength -= len;
					await request.RequestStream.WriteAsync(new FileChunk
					{
						FileName = file.FileName,
						NameSpace = Appcode,
						FileData = ByteString.CopyFrom(buffer, 0, len)
					});
				}
				await request.RequestStream.CompleteAsync();
				fileOpResult = await request.ResponseAsync;
			}
			catch (Exception exc)
			{
				return exc.Message ?? exc.InnerException?.Message ?? "error";
			}
			finally
			{
				stream.Close();
			}

			return string.Empty;
		}
	}
}
