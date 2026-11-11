using MyWebApi.Entities;

namespace MyWebApi.DTOs;

public class CreateGearItemDto
{
    public string Name { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public decimal ReplacementValue { get; set; }
    public bool PassedInspection { get; set; } = true;
}
public class UpdateGearItemDto
{
    public string? Name { get; set; }
    public decimal? DailyRate { get; set; }
    public decimal? ReplacementValue { get; set; }
    public bool? PassedInspection { get; set; }
    public GearStatus? Status { get; set; }
    public GearConditionStatus? Condition { get; set; }
}
public class ReplaceGearItemDto
{
    public string Name { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public decimal ReplacementValue { get; set; }
    public bool PassedInspection { get; set; } = true;
    public string Id { get; set; } = string.Empty;
    public GearStatus Status { get; set; }
    public GearConditionStatus Condition { get; set; }
}