namespace GRPC.Client.Interfaces;

public interface IGreeterClientService
{
	Task<string> CallGreeterMessage(string name, CancellationToken token);
}