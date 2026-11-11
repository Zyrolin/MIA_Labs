using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyWebApi.Entities;

namespace MyWebApi.Documents;

public class UserAccountDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty;
    [BsonElement("accessToken")]
    public string? AccessToken { get; set; }
    [BsonElement("accessTokenExpiryTime")]
    public DateTime? AccessTokenExpirationTime { get; set; }
    [BsonElement("refreshToken")]
    public string? RefreshToken { get; set; }
    [BsonElement("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
    [BsonElement("role")]
    public Roles Role { get; set; } = Roles.None;
}