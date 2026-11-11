using MongoDB.Driver;
using MyWebApi.Documents;
using MyWebApi.Entities;

namespace MyWebApi.Interfaces;

public interface IUserAccountRepository
{
    Task CreateAsync(UserAccount userAccount);
    Task<List<UserAccount>> GetAsync();
    Task<UserAccount?> GetByEmailAsync(string email);
    Task<UserAccount?> GetAsyncById(string id);
    Task UpdateAsync(UserAccount userAccount);
    Task DeleteAsync(string id);
}