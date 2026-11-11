using MyWebApi.Entities;
using MyWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.DTOs;
using MyWebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Controllers;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReserveItemController : ControllerBase
{
    private readonly IReserveItemService _itemService;
    public ReserveItemController(IReserveItemService reserveItemService)
    {
        _itemService = reserveItemService;
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserveitems(ReserveItemDto dto)
    {
        try
        {
            var result = await _itemService.ReserveAsync(dto);
            return CreatedAtRoute("GetContractById", new { id = result.Id }, result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BusinessRuleException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}