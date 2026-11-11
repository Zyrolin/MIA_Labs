using MyWebApi.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using DnsClient.Protocol;

namespace MyWebApi.Documents;

public class GearRentalContractDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("Customer")]
    public Customer Customer { get; set; } = new Customer();
    [BsonElement("Items")]
    public List<GearItem> Items { get; set; } = new List<GearItem>();
    [BsonElement("StartDate")]
    public DateTime StartDate { get; set; }
    [BsonElement("PlannedEndDate")]
    public DateTime PlannedEndDate { get; set; }
    [BsonElement("ActualEndDate")]
    public DateTime? ActualEndDate { get; set; }
    [BsonElement("Createdat")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [BsonElement("Status")]
    public ContractStatus Status { get; set; }
    [BsonElement("TotalRentalCost")]
    public decimal TotalRentalCost { get; set; }
    [BsonElement("TotalPayAmount")]
    public decimal TotalPayAmount { get; set; }
    [BsonElement("DepositAmount")]
    public decimal DepositAmount { get; set; }
    [BsonElement("GiftBonus")]
    public string GiftBonus { get; set; } = string.Empty;
}