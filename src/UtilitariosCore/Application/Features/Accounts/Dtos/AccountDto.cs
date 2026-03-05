using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Accounts.Dtos;

public class AccountPropertyDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
}

public class AccountRenewalDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class AccountDto
{
    public int Id { get; set; }
    public AccountType Type { get; set; }
    public required string Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ProfileUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RecoveryEmail { get; set; }
    public DateTime? LastConnection { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AccountPropertyDto> Properties { get; set; } = [];
    public List<AccountRenewalDto> Renewals { get; set; } = [];
}
