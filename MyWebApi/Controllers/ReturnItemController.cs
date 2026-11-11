using MyWebApi.Entities;
using MyWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Interfaces;
using MyWebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Controllers;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReturnItemController : ControllerBase
{
    private readonly IReturnItemsService _returnItems;
    public ReturnItemController(IReturnItemsService returnItemsService)
    {
        _returnItems = returnItemsService;
    }

    [HttpPut("return")]
    public async Task<ActionResult<GearRentalContract>> ReturnItems([FromBody] ReturnItemsDto request)
    {
        try
        {
            var result = await _returnItems.ReturnAsync(request);
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