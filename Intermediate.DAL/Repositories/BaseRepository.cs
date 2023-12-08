using Intermediate.DAL.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;
using Intermediate.DAL.Extensions;
using MongoDB.Bson;

namespace Intermediate.DAL.Repositories;

public abstract class BaseRepository<T> where T : IPersistable
{
	protected virtual string CollectionName => typeof(T).Name;
	protected virtual IMongoCollection<T> Collection
		=> MongoDatabase.GetCollection<T>(CollectionName);

	protected readonly IMongoDatabase MongoDatabase;

	protected BaseRepository(IDbContext context)
	{
		MongoDatabase = context.Database;
	}

	public virtual IQueryable<T> Query() => Collection.AsQueryable();

	public virtual IQueryable<T> Query(Expression<Func<T, bool>> filter) => Collection.AsQueryable().Where(filter);

	public ValueTask<T> FindAsync(ObjectId id) =>
		id.IsNotEmpty()
			? new ValueTask<T>(Collection.Find(Builders<T>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync())
			: new ValueTask<T>((T)default);

	public async ValueTask<T> GetAsync(ObjectId id)
	{
		var persistable = await FindAsync(id);
		if (persistable == null)
			throw new KeyNotFoundException($"{typeof(T)} id: {id}");
		return persistable;
	}

	public virtual Task SaveAsync(T persistable)
	{
		if (persistable.Id == ObjectId.Empty)
			persistable.Id = ObjectId.GenerateNewId();

		return Collection.FindOneAndReplaceAsync(
			Builders<T>.Filter.Eq(x => x.Id, persistable.Id),
			persistable,
			new FindOneAndReplaceOptions<T>
			{
				IsUpsert = true
			}
		);
	}
}