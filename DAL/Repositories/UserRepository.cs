using System.Linq.Expressions;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : IRepository<AppUser>
{
	private readonly AppDataContext _context;

	public UserRepository(AppDataContext context)
	{
		_context = context;
	}

	public async Task<Guid> Create(AppUser entity, CancellationToken token)
	{
		try
		{
			await _context.AppUsers.AddAsync(entity, token);
			await _context.SaveChangesAsync(token);

			return entity.Id;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public Task<AppUser> Get(Guid id, CancellationToken token)
	{
		return _context.AppUsers.FirstAsync(x => x.Id == id, token);
	}

	public async Task Update(AppUser entity, CancellationToken token)
	{
		_context.Entry(entity).State = EntityState.Modified;

		await _context.SaveChangesAsync(token);
	}

	public async Task Delete(Guid id, CancellationToken token)
	{
		var user = await _context.AppUsers.FirstAsync(x => x.Id == id, token);
		_context.AppUsers.Remove(user);

		await _context.SaveChangesAsync(token);
	}

	public IQueryable<AppUser> Where(Expression<Func<AppUser, bool>> predicate)
	{
		return _context.AppUsers.Where(predicate);
	}
}