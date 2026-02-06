using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Accounts.Dtos;

public class AccountDto
{
    public int Id { get; set; }
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ProfileUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RecoveryEmail { get; set; }
    public DateTime? LastConnection { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAccountDto
{
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ProfileUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RecoveryEmail { get; set; }
    public DateTime? LastConnection { get; set; }
}

public class UpdateAccountDto
{
    public AccountType? Type { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ProfileUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RecoveryEmail { get; set; }
    public DateTime? LastConnection { get; set; }
}
