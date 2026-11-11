using MyWebApi.DTOs;
using MyWebApi.Entities;


namespace MyWebApi.Interfaces;

    
public interface IGearRentalContractRepository
{
    Task<IReadOnlyList<GearRentalContract>> GetAllAsync();
    Task<GearRentalContract?> GetByIdAsync(string id);
    Task<GearRentalContract> CreateAsync(GearRentalContract createDto);
    Task<bool> ReplaceAsync(string id, ReplaceGearRentalContractDto changeDto);
    Task<GearRentalContract?> UpdateAsync(string id, UpdateGearRentalContractDto contr);
    Task<bool> DeleteAsync(string id);
}
