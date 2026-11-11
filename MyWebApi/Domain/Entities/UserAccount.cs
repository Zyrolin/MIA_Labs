
namespace MyWebApi.Entities;

public class UserAccount
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpirationTime { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public Roles Role { get; set; } = Roles.None;
}