using MyWebApi.DTOs;
using MyWebApi.Entities;


namespace MyWebApi.Interfaces;

    
public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(string id);
    Task<Customer?> GetByIdCardAsync(string idCard);
    Task<Customer> CreateAsync(CreateCustomerDto createDto);
    Task<bool> ReplaceAsync(string id, ReplaceCustomerDto changeDto);
    Task<Customer?> UpdateAsync(string id, UpdateCustomerDto updateDto);
    Task<bool> DeleteAsync(string id);
}