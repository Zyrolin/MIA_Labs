using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using System.Runtime.InteropServices;

namespace MyWebApi.Interfaces;

public interface IReturnItemsService
{
    Task<GearRentalContract> ReturnAsync(ReturnItemsDto request);
}