using System.Linq.Expressions;

namespace DAL.Interfaces;

public interface IRepository<T> where T : IPersistable
{
	Task<Guid> Create(T entity, CancellationToken token);
	Task<T> Get(Guid id, CancellationToken token);
	Task Update(T entity, CancellationToken token);
	Task Delete(Guid id, CancellationToken token);
	IQueryable<T> Where(Expression<Func<T, bool>> predicate);
}