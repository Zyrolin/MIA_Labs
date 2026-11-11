using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Entities;
public enum ContractStatus
{
    Reserved,
    Active,
    Completed,
    Cancelled 
}
public class GearRentalContract
{
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = "Покупець обов'язковий")]
    public Customer Customer { get; set; } = new Customer();
    [Required(ErrorMessage = "Список елементів спорядження обов'язковий")]
    public List<GearItem> Items { get; set; } = new List<GearItem>();
    [Required(ErrorMessage = "Дата початку оренди обов'язкова")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Планова дата закінчення оренди обов'язкова")]
    public DateTime PlannedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ContractStatus Status { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Загальна вартість оренди не може бути від'ємною")]
    public decimal TotalRentalCost { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Загальна сума оплати за прокат не може бути від'ємною")]
    public decimal TotalPayAmount { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Сума застави не може бути від'ємною")]
    public decimal DepositAmount { get; set; }
    public string? GiftBonus { get; set; }
    public int PlannedRentalDays => (int)Math.Ceiling((PlannedEndDate - StartDate).TotalDays);
    public int OverdueDays => ActualEndDate is null ? 0 : Math.Max(0, (int)Math.Ceiling((ActualEndDate.Value - PlannedEndDate).TotalDays));
}