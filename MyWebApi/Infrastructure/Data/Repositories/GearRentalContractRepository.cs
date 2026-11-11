using MongoDB.Driver;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Documents;
using MyWebApi.Entities;
using MyWebApi.Interfaces;
using AutoMapper;

namespace MyWebApi.Repositories;

public class GearRentalContractRepository : IGearRentalContractRepository
{
    private readonly IMongoCollection<GearRentalContractDocument> _collection;
    private readonly IMapper _mapper;

    public GearRentalContractRepository(IMapper mapper, MongoDbClient mongoDbClient)
    {
        _collection = mongoDbClient.GetCollection<GearRentalContractDocument>("GearRentalContracts");

        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<GearRentalContract>> GetAllAsync()
    {
        var documents = await _collection.Find(_ => true).ToListAsync();
        return _mapper.Map<List<GearRentalContract>>(documents);
    }
    public async Task<GearRentalContract?> GetByIdAsync(string id)
    {
        var document = await _collection.Find( x => x.Id == id).FirstOrDefaultAsync();
        return document is null ? null : _mapper.Map<GearRentalContract>(document);
    }
    public async Task<GearRentalContract> CreateAsync(GearRentalContract dto)
    {
        var document = _mapper.Map<GearRentalContractDocument>(dto);

        await _collection.InsertOneAsync(document);
        return _mapper.Map<GearRentalContract>(document);
    }
    public async Task<bool> ReplaceAsync(string id, ReplaceGearRentalContractDto dto)
    {
        var document = new GearRentalContractDocument
        {
            Customer = dto.Customer,
            Items = dto.Items,
            StartDate = dto.StartDate,
            PlannedEndDate = dto.PlannedEndDate,
            GiftBonus = dto.GiftBonus,
            TotalPayAmount = dto.TotalPayAmount,
            TotalRentalCost = dto.TotalRentalCost,
            DepositAmount = dto.DepositAmount,
        };
        
        var result = await _collection.ReplaceOneAsync(x => x.Id == id, document);
        return result.MatchedCount > 0;
    }
    public async Task<GearRentalContract?> UpdateAsync(string id, UpdateGearRentalContractDto dto)
    {
    var updates = new List<UpdateDefinition<GearRentalContractDocument>>();
    var update = Builders<GearRentalContractDocument>.Update;

    if (dto.Customer is not null)
        updates.Add(update.Set(x => x.Customer, dto.Customer));

    if (dto.Items is not null)
        updates.Add(update.Set(x => x.Items, dto.Items));

    if (dto.StartDate.HasValue)
        updates.Add(update.Set(x => x.StartDate, dto.StartDate.Value));

    if (dto.PlannedEndDate.HasValue)
        updates.Add(update.Set(x => x.PlannedEndDate, dto.PlannedEndDate.Value));

    if (dto.ActualEndDate.HasValue)
        updates.Add(update.Set(x => x.ActualEndDate, dto.ActualEndDate.Value));

    if (dto.Status.HasValue)
        updates.Add(update.Set(x => x.Status, dto.Status.Value));

    if (dto.TotalRentalCost.HasValue)
        updates.Add(update.Set(x => x.TotalRentalCost, dto.TotalRentalCost.Value));

    if (dto.TotalPayAmount.HasValue)
        updates.Add(update.Set(x => x.TotalPayAmount, dto.TotalPayAmount.Value));

    if (dto.DepositAmount.HasValue)
        updates.Add(update.Set(x => x.DepositAmount, dto.DepositAmount.Value));

    if (dto.GiftBonus is not null)
        updates.Add(update.Set(x => x.GiftBonus, dto.GiftBonus));

    if (updates.Count == 0)
        return await GetByIdAsync(id);

    var options = new FindOneAndUpdateOptions<GearRentalContractDocument>
    {
        ReturnDocument = ReturnDocument.After
    };

    var document = await _collection.FindOneAndUpdateAsync(
        x => x.Id == id,
        update.Combine(updates),
        options);

    return document is null
        ? null
        : _mapper.Map<GearRentalContract>(document);
}
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
public class GearRentalContractProfile : Profile
{
    public GearRentalContractProfile()
    {
        CreateMap<GearRentalContractDocument, GearRentalContract>()
            .ReverseMap();
            
    }
}