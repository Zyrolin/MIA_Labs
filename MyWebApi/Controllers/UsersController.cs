using MyWebApi.Entities;
using MyWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Interfaces;
using MyWebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApi.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Roles.Admin))]
public class UsersController : ControllerBase
{
    private readonly IUserAccountService _userService;
    public UsersController(IUserAccountService userService) => _userService = userService;

    [HttpPost("setRoles")]
    public async Task<IActionResult> SetRoles(string id, Roles roles)
    {
        var user = await _userService.GetAsync(id) ;
        if (user == null) return NotFound("User not found");

        user.Role = roles;
        await _userService.UpdateAsync(user);

        return Ok(new { roles = user.Role, rolesValue = (int)user.Role });
    }

    [HttpPost("addRole")]
    public async Task<IActionResult> AddRole(string id, Roles role)
    {
        var user = await _userService.GetAsync(id);
        if (user == null) return NotFound();

        user.Role |= role;
        await _userService.UpdateAsync(user);

        return Ok(new { roles = user.Role, rolesValue = (int)user.Role });
    }

    [HttpPost("removeRole")]
    public async Task<IActionResult> RemoveRole(string id, Roles role)
    {
        var user = await _userService.GetAsync(id);
        if (user == null) return NotFound();

        user.Role &= ~role;
        await _userService.UpdateAsync(user);

        return Ok(new { roles = user.Role, rolesValue = (int)user.Role });
    }
}