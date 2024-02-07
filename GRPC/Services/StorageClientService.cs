using Google.Protobuf;
using GRPC.Client.Interfaces;
using static GRPC.Client.FileOP;

namespace GRPC.Client.Services
{
	public class StorageClientService : IStorageClientService
	{
		private readonly FileOPClient _fileOPClient;

		private const string Appcode = "grpcClient";

		public StorageClientService(FileOPClient fileOPClient)
		{
			_fileOPClient = fileOPClient;
		}

		public async Task UploadFile(IFormFile file)
		{
			var stream = file.OpenReadStream();

			var totallength = file.Length;
			var clientCall = _fileOPClient.UploadFile();
			var RequestStream = clientCall.RequestStream;
			var buffer = new byte[1024 * 1024];
			FileUploadStatus fileOpResult;
			try
			{
				// Will send the files to be up to the GRPC server
				while (totallength > 0)
				{
					var len = await stream.ReadAsync(buffer);
					totallength -= len;
					await RequestStream.WriteAsync(new FileChunk
					{
						FileName = file.FileName,
						NameSpace = Appcode,
						FileData = ByteString.CopyFrom(buffer, 0, len)
					});
				}
				await RequestStream.CompleteAsync();
				fileOpResult = await clientCall.ResponseAsync;
			}
			finally
			{
				stream.Close();
				stream.Dispose();
				clientCall.Dispose();
			}
		}
	}
}
