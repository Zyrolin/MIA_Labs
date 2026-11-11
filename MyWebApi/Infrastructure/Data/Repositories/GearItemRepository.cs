using MongoDB.Driver;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Documents;
using MyWebApi.Entities;
using MyWebApi.Interfaces;
using AutoMapper;

namespace MyWebApi.Repositories;

public class GearItemRepository : IGearItemRepository
{
    private readonly IMongoCollection<GearItemDocument> _collection;
    private readonly IMapper _mapper;

    public GearItemRepository(IMapper mapper, MongoDbClient mongoDbClient)
    {
        _collection = mongoDbClient.GetCollection<GearItemDocument>("GearItems");

        _mapper = mapper;
    }
    public async Task<IReadOnlyList<GearItem>> GetAllAsync()
    {
        var documents = await _collection.Find(_ => true).ToListAsync();
        return _mapper.Map<List<GearItem>>(documents);
    }
    public async Task<GearItem?> GetByIdAsync(string id)
    {
        var document = await _collection.Find( x => x.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<GearItem>(document) is null ? null : _mapper.Map<GearItem>(document);
    }
    public async Task<GearItem> CreateAsync(CreateGearItemDto dto)
    {
        var document = new GearItemDocument
        {
            Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            Name = dto.Name,
            ReplacementValue = dto.ReplacementValue,
            DailyRate = dto.DailyRate,
            PassedInspection = dto.PassedInspection,
        };

        await _collection.InsertOneAsync(document);
        return _mapper.Map<GearItem>(document);
    }
    public async Task<bool> ReplaceAsync(string id, ReplaceGearItemDto dto)
    {
        var document = new GearItemDocument
        {
            Id = id,
            Name = dto.Name,
            ReplacementValue = dto.ReplacementValue,
            DailyRate = dto.DailyRate,
            PassedInspection = dto.PassedInspection,
            Status = dto.Status,
            Condition = dto.Condition
        };
        
        var result = await _collection.ReplaceOneAsync(x => x.Id == id, document);
        return result.MatchedCount > 0;
    }
    public async Task<GearItem?> UpdateAsync(string id, UpdateGearItemDto dto)
    {
        var updates = new List<UpdateDefinition<GearItemDocument>>();
        var update = Builders<GearItemDocument>.Update;

        if (dto.Name is not null)
            updates.Add(update.Set(x => x.Name, dto.Name));

        if (dto.DailyRate.HasValue)
            updates.Add(update.Set(x => x.DailyRate, dto.DailyRate));

        if (dto.ReplacementValue.HasValue)
            updates.Add(update.Set(x => x.ReplacementValue, dto.ReplacementValue));

        if (dto.PassedInspection.HasValue)
            updates.Add(update.Set(x => x.PassedInspection, dto.PassedInspection));

        if (dto.Status.HasValue)
            updates.Add(update.Set(x => x.Status, dto.Status));

        if (dto.Condition.HasValue)
            updates.Add(update.Set(x => x.Condition, dto.Condition.Value));

        if (updates.Count == 0)
            return await GetByIdAsync(id);

        var options = new FindOneAndUpdateOptions<GearItemDocument>
        {
            ReturnDocument = ReturnDocument.After
        };

        var document = await _collection.FindOneAndUpdateAsync(
            x => x.Id == id,
            update.Combine(updates),
            options);

        return _mapper.Map<GearItem>(document);
    }
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
public class GearItemProfile : Profile
{
    public GearItemProfile()
    {
        CreateMap<GearItemDocument, GearItem>()
            .ReverseMap();
    }
}