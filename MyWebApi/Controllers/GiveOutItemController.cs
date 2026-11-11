using MyWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.DTOs;
using MyWebApi.Interfaces;
using MyWebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GiveOutItem : ControllerBase
{
    private IGiveOutItemService _giveOut;
    public GiveOutItem(IGiveOutItemService giveOutItemService)
    {
        _giveOut = giveOutItemService;
    }

    [HttpPut("give-out")]
    public async Task<ActionResult<GearRentalContract>> GiveOutItems([FromBody] GiveOutItemsDto request)
    {
        try
        {
             var result = await _giveOut.GiveOutItemAsync(request);
            return Ok(result);
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