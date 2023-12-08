using MongoDB.Driver;

namespace Intermediate.DAL.Interfaces
{
    public interface IDbContext
    {
        public IMongoDatabase Database { get; }
    }
}
