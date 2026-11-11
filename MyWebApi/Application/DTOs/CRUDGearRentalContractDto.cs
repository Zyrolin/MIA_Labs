using MyWebApi.Entities;

namespace MyWebApi.DTOs;

public class UpdateGearRentalContractDto
{
    public Customer? Customer { get; set; }
    public List<GearItem>? Items { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public ContractStatus? Status { get; set; }
    public decimal? TotalRentalCost { get; set; }
    public decimal? TotalPayAmount { get; set; }
    public decimal? DepositAmount { get; set; }
    public string? GiftBonus { get; set; }
}
public class ReplaceGearRentalContractDto
{
    public string Id { get; set; } = string.Empty;
    public Customer Customer { get; set; } = new Customer();
    public List<GearItem> Items { get; set; } = new List<GearItem>();
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public ContractStatus Status { get; set; }
    public decimal TotalRentalCost { get; set; }
    public decimal TotalPayAmount { get; set; }
    public decimal DepositAmount { get; set; }
    public string GiftBonus { get; set; } = string.Empty;
}