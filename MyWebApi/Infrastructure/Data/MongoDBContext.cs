using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace MyWebApi.Data;

public class MongoDbClient
{
	private readonly IMongoDatabase _db;

    public MongoDbClient(IOptions<MongoDbSettings> options)
	{
        var connectionString = options.Value.ConnectionString;
        var client = new MongoClient(connectionString);
        var databaseName = options.Value.DatabaseName;
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("MongoDB database name is not configured.");
        }

        _db = client.GetDatabase(databaseName);

    }

	public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
}