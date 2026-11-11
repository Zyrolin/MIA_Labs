using System.ComponentModel.DataAnnotations;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using MyWebApi.Interfaces;
using MyWebApi.Exceptions;

namespace MyWebApi.Services;

public class RentalService : IRentalService
{
    private readonly IGearItemRepository _gearItems;
    private readonly ICustomerRepository _customers;
    private readonly IGearRentalContractRepository _contracts;

    public RentalService(
        IGearItemRepository gearItemRepository,
        ICustomerRepository customerRepository,
        IGearRentalContractRepository gearRentalContractRepository)
    {
        _gearItems = gearItemRepository;
        _customers = customerRepository;
        _contracts = gearRentalContractRepository;
    }

    public Task<IReadOnlyList<GearRentalContract>> GetContractsAsync() =>
        _contracts.GetAllAsync();

    public Task<GearRentalContract?> GetContractByIdAsync(string id) =>
        _contracts.GetByIdAsync(id);

    public Task<IReadOnlyList<GearItem>> GetGearItemsAsync() =>
        _gearItems.GetAllAsync();

    public Task<GearItem?> GetGearItemByIdAsync(string id) =>
        _gearItems.GetByIdAsync(id);

    public Task<IReadOnlyList<Customer>> GetCustomersAsync() =>
        _customers.GetAllAsync();

    public Task<Customer?> GetCustomerByIdAsync(string id) =>
        _customers.GetByIdAsync(id);

    public Task<GearItem> CreateGearItemAsync(CreateGearItemDto item) =>
        _gearItems.CreateAsync(item);

    public Task<Customer> CreateCustomerAsync(CreateCustomerDto customer) =>
        _customers.CreateAsync(customer);
    
    public async Task<decimal?> GetAverageDailyRateAsync()
    {
        var items = await _gearItems.GetAllAsync();
        if (items.Count == 0)
            return null;
        return items.Average(item => item.DailyRate);
    }
    public async Task<bool> ReplaceGearItemAsync(string id, ReplaceGearItemDto item)
    {
        var replaced = await _gearItems.ReplaceAsync(id, item);
        if (!replaced)
            throw new NotFoundException($"Не можу знайти спорядження з таким id = {id}");
        return true;
    }
    public async Task<bool> DeleteGearItemAsync(string id)
    {
        var deleted = await _gearItems.DeleteAsync(id);
        if (!deleted)
            throw new NotFoundException($"Не можу видалити спорядження з таким id = {id}");
        return true;
    }

}
