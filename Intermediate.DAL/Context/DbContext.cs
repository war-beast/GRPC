using System.Text.RegularExpressions;
using Intermediate.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Intermediate.DAL.Context
{
    public class DbContext : IDbContext
    {
        public IMongoDatabase Database { get; }
        public DbContext(IOptions<DatabaseOptions> databaseOptions, ILogger<DbContext> logger)
        {
            Database = MongoDatabaseFactory.CreateDatabase(databaseOptions.Value.Address, databaseOptions.Value.Name);
            var connectionString = databaseOptions.Value.Address;

            try
            {
                var isConnected = Database.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}").Wait(10_000);
                if (isConnected)
	                logger.LogInformation("Успешное соединений к базе данных {server}", connectionString);
                else
                    logger.LogCritical("БД не пингуется {server}", connectionString);
            }
            catch (Exception ex)
            {
	            logger.LogCritical(ex, "Ошибка подключения к базе данных {server}", connectionString);
            }
        }
    }
}
