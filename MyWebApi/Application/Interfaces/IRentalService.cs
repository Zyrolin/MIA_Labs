using MyWebApi.DTOs;
using MyWebApi.Entities;

namespace MyWebApi.Interfaces;

public interface IRentalService
{
    Task<IReadOnlyList<GearRentalContract>> GetContractsAsync();
    Task<GearRentalContract?> GetContractByIdAsync(string id);
    Task<IReadOnlyList<GearItem>> GetGearItemsAsync();
    Task<GearItem?> GetGearItemByIdAsync(string id);
    Task<IReadOnlyList<Customer>> GetCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(string id);
    Task<GearItem> CreateGearItemAsync(CreateGearItemDto item);
    Task<Customer> CreateCustomerAsync(CreateCustomerDto customer);
    Task<bool> ReplaceGearItemAsync(string id, ReplaceGearItemDto item);
    Task<bool> DeleteGearItemAsync(string id);
    Task<decimal?> GetAverageDailyRateAsync();
}
