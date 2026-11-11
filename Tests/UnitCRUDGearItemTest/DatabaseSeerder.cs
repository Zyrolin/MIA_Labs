using MongoDB.Bson;
using MongoDB.Driver;
using MyWebApi.Documents;
using MyWebApi.Entities;

namespace MyWebApi.Tests;
public class DatabaseSeeder
{
    private readonly IMongoDatabase _db;

    public DatabaseSeeder(IMongoDatabase db)
    {
        _db = db;
    }

    public async Task SeedTodosAsync()
    {
        var collection = _db.GetCollection<GearItemDocument>("GearItems");

        await collection.DeleteManyAsync(FilterDefinition<GearItemDocument>.Empty);

        var items = new List<GearItemDocument>
        {
            new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Tent",
                DailyRate = 100m,
                ReplacementValue = 1000m,
                Status = GearStatus.Available,
                PassedInspection = true,
                Condition = GearConditionStatus.Good
            },
            new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Test Backpack",
                DailyRate = 50m,
                ReplacementValue = 500m,
                Status = GearStatus.Available,
                PassedInspection = true,
                Condition = GearConditionStatus.Good
            }
        };

        await collection.InsertManyAsync(items);
    }
}