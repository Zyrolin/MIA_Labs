using MongoDB.Driver;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Documents;
using MyWebApi.Entities;
using MyWebApi.Interfaces;
using AutoMapper;

namespace MyWebApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<CustomerDocument> _collection;
    private readonly IMapper _mapper;

    public CustomerRepository(IMapper mapper, MongoDbClient mongoDbClient)
    {
        _collection = mongoDbClient.GetCollection<CustomerDocument>("Customers");

        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<Customer>> GetAllAsync()
    {
        var documents = await _collection.Find(_ => true).ToListAsync();
        return _mapper.Map<List<Customer>>(documents);
    }
    public async Task<Customer?> GetByIdAsync(string id)
    {
        var document = await _collection.Find( x => x.UserId == id).FirstOrDefaultAsync();
        return _mapper.Map<Customer>(document) is null ? null : _mapper.Map<Customer>(document);
    }
    public async Task<Customer?> GetByIdCardAsync(string idCard)
    {
        var document = await _collection.Find( x => x.IdCard == idCard).FirstOrDefaultAsync();
        return document is null ? null : _mapper.Map<Customer>(document);
    }
    public async Task<Customer> CreateAsync(CreateCustomerDto dto)
    {
        var id = GetByIdAsync(dto.UserId);
        var document = new CustomerDocument
        {
            UserId = id is null ? MongoDB.Bson.ObjectId.GenerateNewId().ToString(): dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IdCard = dto.IdCard,
            CardBalance = dto.CardBalance,
        };

        await _collection.InsertOneAsync(document);
        return _mapper.Map<Customer>(document);
    }
    public async Task<bool> ReplaceAsync(string id, ReplaceCustomerDto dto)
    {
        var document = new CustomerDocument
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IdCard = dto.IdCard,
            BlockedDeposit = dto.BlockedDeposit,
            CardBalance = dto.CardBalance,
            OutstandingDebt = dto.OutstandingDebt,
        };
        
        var result = await _collection.ReplaceOneAsync(x => x.UserId == id, document);
        return result.MatchedCount > 0;
    }
    public async Task<Customer?> UpdateAsync(string id, UpdateCustomerDto dto)
    {
        var updates = new List<UpdateDefinition<CustomerDocument>>();
        var update = Builders<CustomerDocument>.Update;

        if (dto.FirstName is not null)
            updates.Add(update.Set(x => x.FirstName, dto.FirstName));

        if (dto.LastName is not null)
            updates.Add(update.Set(x => x.LastName, dto.LastName));

        if (dto.IdCard is not null)
            updates.Add(update.Set(x => x.IdCard, dto.IdCard));
        
        if (dto.CardBalance.HasValue)
            updates.Add(update.Set(x => x.CardBalance, dto.CardBalance.Value));

        if (dto.BlockedDeposit.HasValue)
            updates.Add(update.Set(x => x.BlockedDeposit, dto.BlockedDeposit.Value));

        if (dto.OutstandingDebt.HasValue)
            updates.Add(update.Set(x => x.OutstandingDebt, dto.OutstandingDebt.Value));
        if (updates.Count == 0)
            return await GetByIdAsync(id);

        var options = new FindOneAndUpdateOptions<CustomerDocument>
        {
            ReturnDocument = ReturnDocument.After
        };

        var document = await _collection.FindOneAndUpdateAsync(
            x => x.UserId == id,
            update.Combine(updates),
            options);

        return _mapper.Map<Customer>(document);
    }
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.UserId == id);
        return result.DeletedCount > 0;
    }
}
public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerDocument, Customer>()
            .ForMember(dest => dest.FullName, opt => 
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ReverseMap();
            
    }
}