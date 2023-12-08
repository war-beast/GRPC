using Intermediate.DAL.Entities;

namespace Intermediate.DAL.Interfaces;

public interface IRequestRepository
{
	Task SaveRequest(Request request);
}