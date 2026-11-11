using System.ComponentModel.DataAnnotations;
using MyWebApi.Entities;

namespace MyWebApi.DTOs;

public class GiveOutItemsDto
{
    [Required(ErrorMessage = "Введіть id контракту")]
    public string contractId { get; set; } = string.Empty;
    [Required(ErrorMessage = "Введіть id картку клієнта")]
    public string customerIdCard { get; set; } = string.Empty;
}
public class ReserveItemDto
{
    public string CustomerIdCard { get; set; } = string.Empty;
    public List<string> ItemsId { get; set; } = new List<string>();
    public DateTime PlannedEndDate { get; set; }
}
public class ReturnItemsDto
{
    public string? contractId { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public List<GearConditionStatus>? gearConditions { get; set; }
}