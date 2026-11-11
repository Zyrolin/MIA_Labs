using MyWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = nameof(Roles.Admin))]
public class RentalServiceController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalServiceController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet("contract")]
    public async Task<ActionResult<IReadOnlyList<GearRentalContract>>> GetContracts()
    {
        var contracts = await _rentalService.GetContractsAsync();
        return Ok(contracts);
    }

    [HttpGet("gear")]
    public async Task<ActionResult<IReadOnlyList<GearItem>>> GetGearsAsync()
    {
        var gearItems = await _rentalService.GetGearItemsAsync();
        return Ok(gearItems);
    }

    [HttpGet("customer")]
    public async Task<ActionResult<IReadOnlyList<Customer>>> GetCustomers()
    {
        var customers = await _rentalService.GetCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("contract/{id}", Name = "GetContractById")]
    public async Task<ActionResult<GearRentalContract>> GetContractById(string id)
    {
        var contract = await _rentalService.GetContractByIdAsync(id);
        if (contract == null)
        {
            return NotFound($"Контракт з id={id} не знайдено");
        }
        return Ok(contract);
    }

    [HttpGet("gear/{id}", Name = "GetGearitemById")]
    public async Task<ActionResult<GearItem>> GetGearItemById(string id)
    {
        var item = await _rentalService.GetGearItemByIdAsync(id);
        if (item == null)
        {
            return NotFound($"Елемент спорядження з id={id} не знайдено");
        }
        return Ok(item);
    }

    [HttpGet("customer/{id}", Name = "GetCustomerById")]
    public async Task<ActionResult<Customer>> GetCustomerById(string id)
    {
        var customer = await _rentalService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound($"Клієнт з id={id} не знайдено");
        }
        return Ok(customer);
    }

    [HttpPost("gear")]
    public async Task<ActionResult<GearItem>> CreateGearItem([FromBody] CreateGearItemDto item)
    {
        var result = await _rentalService.CreateGearItemAsync(item);
        return CreatedAtAction(nameof(GetGearItemById), new { id = result.Id }, result);
    }

    [HttpPost("customer")]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerDto cust)
    {
        var result = await _rentalService.CreateCustomerAsync(cust);
        return CreatedAtAction(nameof(GetCustomerById), new { id = result.UserId }, result);
    }
    [HttpPut("gear/{id}")]
    public async Task<ActionResult> ReplaceGearItem([FromRoute] string id, [FromBody] ReplaceGearItemDto item)
    {
        await _rentalService.ReplaceGearItemAsync(id, item);
        return NoContent();
    }
    [HttpDelete("gear/{id}")]
    public async Task<ActionResult> DeleteGearItem([FromRoute] string id)
    {
        await _rentalService.DeleteGearItemAsync(id);
        return NoContent();
    }
}
