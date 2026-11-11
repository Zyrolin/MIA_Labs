using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyWebApi.Entities;

namespace MyWebApi.Documents;

public class CustomerDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId{ get; set; } = string.Empty;
    [BsonElement("FirstName")]
    public string FirstName { get; set; } = string.Empty;
    [BsonElement("LastName")]
    public string LastName { get; set; } = string.Empty;
    [BsonElement("IdCard")]
    public string IdCard { get; set; } = string.Empty;
    [BsonElement("CardBalance")]
    public decimal CardBalance { get; set; }
    [BsonElement("BlockedDeposit")]
    public decimal BlockedDeposit { get; set; }
    [BsonElement("OutstandingDebt")]
    public decimal OutstandingDebt { get; set; }
};
