using GRPC.Client.Models;

namespace GRPC.Client.Interfaces;

public interface IUserService
{
	Task<string> GetToken(UserRequest request, CancellationToken token);

	Task<Guid> Add(AddUserRequest request, CancellationToken token);
}