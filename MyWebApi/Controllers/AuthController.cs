using Microsoft.AspNetCore.Mvc;
using MyWebApi.Entities;
using MyWebApi.Services;
using MyWebApi.Interfaces;
using MyWebApi.DTOs;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using MyWebApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyWebApi.Controllers;

public class LoginModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class RegisterModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCard { get; set; } = string.Empty;

}
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAccountService _userAccountService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtSettings _jwtSettings;
    private readonly ICustomerRepository _customers;
    public AuthController(IUserAccountService UserAccountService, JwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher, IOptions<JwtSettings> jwtOptions, ICustomerRepository customerRepository)
    {
        _userAccountService = UserAccountService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtOptions.Value;
        _customers = customerRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel user)
    {
        var existingUserAccount = await _userAccountService.GetByEmailAsync(user.Email);
        if (existingUserAccount != null)
            return BadRequest("Користувач уже існує.");

        var id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        UserAccount newUser = new();
        newUser.Username = user.Username;
        newUser.PasswordHash = _passwordHasher.HashPassword(user.Password);
        newUser.Id = id;
        newUser.Email = user.Email;
        newUser.Role = Roles.Customer;
        await _userAccountService.CreateAsync(newUser);

        CreateCustomerDto newCustomer = new();
        newCustomer.FirstName = user.FirstName;
        newCustomer.LastName = user.LastName;
        newCustomer.UserId = id;
        newCustomer.CardBalance = 10000;
        newCustomer.IdCard = user.IdCard;
        await _customers.CreateAsync(newCustomer);

        return Ok("Користувач зареєстрований.");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userAccountService.GetByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized("Неправильна пошта або пароль.");

        if (!_passwordHasher.Verify(model.Password, user.PasswordHash))
            return Unauthorized("Неправильний пароль.");

        var token = await _jwtTokenGenerator.Generate(user);

        user.AccessToken = token;
        user.AccessTokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        await _userAccountService.UpdateAsync(user);

        return Ok(new {
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] string authHeader, [FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return BadRequest("Missing or invalid Authorization header");

        var token = authHeader.Substring("Bearer ".Length);

        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);

        if (principal == null)
            return BadRequest("Invalid refresh token");

        var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id == null) throw new Exception("Invalid token claims");

        var user = await _userAccountService.GetAsync(id);

        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid refresh token");
        }

        var newAccessToken = await _jwtTokenGenerator.Generate(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.AccessToken = newAccessToken;
        user.AccessTokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        await _userAccountService.UpdateAsync(user);

        return Ok(new
        {
            token = newAccessToken,
            refreshToken = newRefreshToken,
            TokenExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }
}