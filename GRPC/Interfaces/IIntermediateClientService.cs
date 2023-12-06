namespace GRPC.Client.Interfaces;

public interface IIntermediateClientService
{
	Task<bool> CallIntermediateMessage(ResendRequest request, string jwt, CancellationToken token);
}