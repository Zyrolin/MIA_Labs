using MyWebApi.DTOs;
using MyWebApi.Entities;


namespace MyWebApi.Interfaces;

    
public interface IGearItemRepository
{
    Task<IReadOnlyList<GearItem>> GetAllAsync();
    Task<GearItem?> GetByIdAsync(string id);
    Task<GearItem> CreateAsync(CreateGearItemDto createDto);
    Task<bool> ReplaceAsync(string id, ReplaceGearItemDto changeDto);
    Task<GearItem?> UpdateAsync(string id, UpdateGearItemDto updateDto);
    Task<bool> DeleteAsync(string id);
}
