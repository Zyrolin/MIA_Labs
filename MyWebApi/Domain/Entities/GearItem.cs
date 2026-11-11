using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Entities;
public enum GearStatus
{
    Available,
    Maintence,
    Reserved,
    RentedOut
}
public enum GearConditionStatus
{
    Good,
    Broken,
    CanNotBeUsed
}

public class GearItem
{
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = "Назва елемента спорядження обов'язкова")]
    [StringLength(40, MinimumLength = 1, ErrorMessage = "Назва елемента спорядження повинна бути від 1 до 40 символів")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Ціна за добу обов'язкова")]
    public decimal DailyRate { get; set; }
    [Required(ErrorMessage = "Вартість заміни обов'язкова")]
    public decimal ReplacementValue { get; set; }
    public GearStatus Status { get; set; } = GearStatus.Available;
    public bool PassedInspection { get; set; } = true;
    public GearConditionStatus Condition { get; set; } = GearConditionStatus.Good;
}