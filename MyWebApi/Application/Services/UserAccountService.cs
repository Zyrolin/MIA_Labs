using MyWebApi.Entities;
using MyWebApi.Interfaces;

namespace MyWebApi.Services;
public class UserAccountService : IUserAccountService
{
    private readonly IUserAccountRepository _userRepository;

    public UserAccountService(IUserAccountRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateAsync(UserAccount UserAccount) => await _userRepository.CreateAsync(UserAccount);
    public async Task DeleteAsync(string id) => await _userRepository.DeleteAsync(id);
    public async Task<List<UserAccount>> GetAsync() => await _userRepository.GetAsync();
    public async Task<UserAccount?> GetByEmailAsync(string email) => await _userRepository.GetByEmailAsync(email);
    public async Task<UserAccount?> GetAsync(string id) => await _userRepository.GetAsyncById(id);
    public async Task UpdateAsync(UserAccount UserAccount) => await _userRepository.UpdateAsync(UserAccount);
}
