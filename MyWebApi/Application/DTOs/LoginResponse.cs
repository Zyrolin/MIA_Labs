using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace MyWebApi.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime TokenExpiryTime { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set;}
}