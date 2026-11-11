namespace MyWebApi.DTOs;

public class CreateCustomerDto
{
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCard { get; set; } = string.Empty;
    public decimal CardBalance { get; set; }
}
public class UpdateCustomerDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? IdCard { get; set; }
    public decimal? CardBalance { get; set; }
    public decimal? BlockedDeposit { get; set; }
    public decimal? OutstandingDebt { get; set; }
}
public class ReplaceCustomerDto
{
    public string Id{ get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCard { get; set; } = string.Empty;
    public decimal CardBalance { get; set; }
    public decimal BlockedDeposit { get; set; }
    public decimal OutstandingDebt { get; set; }
}