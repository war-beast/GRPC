using FileService;
using Grpc.Core;
using Minio.AspNetCore;
using Minio.DataModel.Args;

namespace FileUploader.Services
{
	public class UploaderService : FileOP.FileOPBase
	{
		public readonly IMinioClientFactory _minioClientFactory;

		public UploaderService(IMinioClientFactory minioClientFactory)
		{
			_minioClientFactory = minioClientFactory;
		}

		public override async Task<FileUploadStatus> UploadFile(IAsyncStreamReader<FileChunk> requestStream, ServerCallContext context)
		{
			var bucketName = "grpc-learning";
			while (await requestStream.MoveNext())
			{
				var filename = requestStream.Current.FileName;

				using var fileStream = new MemoryStream();
				requestStream.Current.FileData.WriteTo(fileStream);
				var fileBytes = fileStream.ToArray();

				var client = _minioClientFactory.CreateClient("grpc");

				try{
					var putObjectArgs = new PutObjectArgs()
						.WithBucket(bucketName)
						.WithObject(filename)
						.WithStreamData(new MemoryStream(fileStream.GetBuffer()))
						.WithObjectSize(fileStream.Length)
						.WithContentType("application/octet-stream");
					await client.PutObjectAsync(putObjectArgs);
				}
				catch (Exception ex) {
					var ms = ex.Message;
				}
			}

			return new FileUploadStatus { FileId = 1, Msg = "ок", Ok = true };
		}
	}
}
