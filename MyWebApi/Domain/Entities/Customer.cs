using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Entities;
public class Customer
{
    public string UserId{ get; set; } = string.Empty;
    [Required(ErrorMessage = "Повне ім'я обов'язкове")]
    public string? FullName { get; set; }
    [Required(ErrorMessage = "Номер ID-картки обов'язковий")]
    [RegularExpression(@"\d{10}$", ErrorMessage = "Номер ID-картки повинен містити 10 цифр")]
    public string IdCard { get; set; } = string.Empty;
    [Required(ErrorMessage = "Баланс картки обов'язковий")]
    [Range(0, double.MaxValue, ErrorMessage = "Баланс картки не може бути від'ємним")]
    public decimal CardBalance { get; set; }
    public decimal BlockedDeposit { get; set; }
    public decimal OutstandingDebt { get; set; }
};
