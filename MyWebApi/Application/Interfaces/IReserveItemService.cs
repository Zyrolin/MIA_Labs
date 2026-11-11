using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using System.Runtime.InteropServices;

namespace MyWebApi.Interfaces;

public interface IReserveItemService
{
    Task<GearRentalContract> ReserveAsync(ReserveItemDto request);
}