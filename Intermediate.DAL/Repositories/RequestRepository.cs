using Intermediate.DAL.Entities;
using Intermediate.DAL.Interfaces;

namespace Intermediate.DAL.Repositories;

public class RequestRepository : BaseRepository<Request>, IRequestRepository
{
	public RequestRepository(IDbContext context) : base(context)
	{
	}

	public Task SaveRequest(Request request)
	{
		return SaveAsync(request);
	}
}