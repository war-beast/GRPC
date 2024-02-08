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

				var fileBytes = requestStream.Current.FileData.ToArray();

				var client = _minioClientFactory.CreateClient("grpc");

				try{
					var putObjectArgs = new PutObjectArgs()
						.WithBucket(bucketName)
						.WithObject(filename)
						.WithStreamData(new MemoryStream(fileBytes))
						.WithObjectSize(fileBytes.Length)
						.WithContentType("application/octet-stream");
					await client.PutObjectAsync(putObjectArgs);
				}
				catch (Exception ex) {
					return new FileUploadStatus { FileName = filename, Msg = ex.Message, Ok = false };
				}
			}

			return new FileUploadStatus { FileName = string.Empty, Msg = "ок", Ok = true };
		}
	}
}
