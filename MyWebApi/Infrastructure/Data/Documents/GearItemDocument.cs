using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AutoMapper;
using MyWebApi.Entities;

namespace MyWebApi.Documents;

public class GearItemDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("dailyRate")]
    public decimal DailyRate { get; set; }
    [BsonElement("replacementValue")]
    public decimal ReplacementValue { get; set; }
    [BsonElement("status")]
    public GearStatus Status { get; set; }
    [BsonElement("passedInspection")]
    public bool PassedInspection { get; set; } = true;
    [BsonElement("condition")]
    public GearConditionStatus Condition { get; set; }
};
