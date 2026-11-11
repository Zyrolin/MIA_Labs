using MongoDB.Driver;
using MyWebApi.Interfaces;
using MyWebApi.Documents;
using MyWebApi.Data;
using MyWebApi.Entities;
using AutoMapper;

namespace MyWebApi.Repositories;

public class UserAccountRepository: IUserAccountRepository
{
    private readonly IMongoCollection<UserAccountDocument> _collection;
    private readonly IMapper _mapper;

    public UserAccountRepository(IMapper mapper, MongoDbClient mongoDbClient)
    {
        _collection = mongoDbClient.GetCollection<UserAccountDocument>("Users");
        _mapper = mapper;
    }

    public async Task<UserAccount?> GetAsyncById(string id)
    {
        var document = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        return document is null ? null : _mapper.Map<UserAccount>(document);
    }

    public async Task<UserAccount?> GetByEmailAsync(string email)
    {
        var document = await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        return document is null ? null : _mapper.Map<UserAccount>(document);
    }

    public async Task<List<UserAccount>> GetAsync()
    {
        var documents = await _collection.Find(x => true).ToListAsync();
        return documents.Select(_mapper.Map<UserAccount>).ToList();
    }

    public async Task CreateAsync(UserAccount userAccount)
        => await _collection.InsertOneAsync(_mapper.Map<UserAccountDocument>(userAccount));

    public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task UpdateAsync(UserAccount userAccount)
        => await _collection.ReplaceOneAsync(x => x.Id == userAccount.Id, _mapper.Map<UserAccountDocument>(userAccount));

}
   public class UserAccountProfile : Profile
{
    public UserAccountProfile()
    {
        CreateMap<UserAccountDocument, UserAccount>()
            .ReverseMap();
            
    }
}