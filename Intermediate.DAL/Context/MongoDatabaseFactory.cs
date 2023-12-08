using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Intermediate.DAL.Context
{
    public class MongoDatabaseFactory
    {
        public static IMongoDatabase CreateDatabase(string connectionString,
            string databaseName)
        {
            var mongoClient = new MongoClient(connectionString);
            var pack = new ConventionPack(){
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("My test convention", pack, t => true);
            return mongoClient.GetDatabase(databaseName);
        }
    }
}
