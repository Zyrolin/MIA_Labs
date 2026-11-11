using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using System.Runtime.InteropServices;

namespace MyWebApi.Interfaces;

public interface IGiveOutItemService
{
    public Task<GearRentalContract>GiveOutItemAsync(GiveOutItemsDto request);
}