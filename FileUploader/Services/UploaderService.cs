using FileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Minio.AspNetCore;
using Minio.DataModel.Args;
using System.Reactive.Linq;

namespace FileUploader.Services
{
	public class UploaderService : FileOP.FileOPBase
	{
		private readonly IMinioClientFactory _minioClientFactory;
		private readonly ILogger _logger;

		private const string BucketName = "grpc-learning";

		public UploaderService(IMinioClientFactory minioClientFactory, 
			ILogger logger)
		{
			_minioClientFactory = minioClientFactory;
			_logger = logger;
		}

		public override async Task<FileUploadStatus> UploadFile(IAsyncStreamReader<FileChunk> requestStream, ServerCallContext context)
		{
			while (await requestStream.MoveNext())
			{
				var filename = requestStream.Current.FileName;

				var fileBytes = requestStream.Current.FileData.ToArray();

				var client = _minioClientFactory.CreateClient("grpc");

				try
				{
					var putObjectArgs = new PutObjectArgs()
						.WithBucket(BucketName)
						.WithObject(filename)
						.WithStreamData(new MemoryStream(fileBytes))
						.WithObjectSize(fileBytes.Length)
						.WithContentType("application/octet-stream");
					await client.PutObjectAsync(putObjectArgs);
				}
				catch (Exception ex)
				{
					return new FileUploadStatus { FileName = filename, Msg = ex.Message, Ok = false };
				}
			}

			return new FileUploadStatus { FileName = string.Empty, Msg = "ок", Ok = true };
		}

		public override async Task GetFilesList(Empty request,
			IServerStreamWriter<FileList> responseStream,
			ServerCallContext context)
		{
			var client = _minioClientFactory.CreateClient("grpc");
			var listArgs = new ListObjectsArgs { IsBucketCreationRequest = false }
				.WithBucket(BucketName);

			try
			{
				var files = await client.ListObjectsAsync(listArgs, context.CancellationToken).ToArray();

				foreach (var file in files)
				{
					await responseStream.WriteAsync(new FileList { FileName = file.Key });
				}
			}
			catch (Exception exc)
			{
				_logger.LogError(exc, "Ошибка при получении списка файлов.");
			}
		}
	}
}
