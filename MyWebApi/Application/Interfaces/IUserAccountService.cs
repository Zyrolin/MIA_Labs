using MyWebApi.Entities;

namespace MyWebApi.Interfaces;

public interface IUserAccountService
{
    Task CreateAsync(UserAccount UserAccount);
    Task<List<UserAccount>> GetAsync();
    Task<UserAccount?> GetByEmailAsync(string email);
    Task<UserAccount?> GetAsync(string id);
    Task UpdateAsync(UserAccount UserAccount);
    Task DeleteAsync(string id);
}