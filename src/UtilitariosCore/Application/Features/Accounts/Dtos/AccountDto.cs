using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Accounts.Dtos;

public class AccountEmailDto
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? RecoveryEmailId { get; set; }
    public string? RecoveryEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountSteamDto
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string? EmailAddress { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfileUrl { get; set; }
    public bool HasDota2 { get; set; }
    public bool HasCS2 { get; set; }
    public bool IsUnlimited { get; set; }
    public bool IsVacBanned { get; set; }
    public bool HasSteamMobile { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountGitHubDto
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string? EmailAddress { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountGeneralDto
{
    public int Id { get; set; }
    public GeneralPlatform Platform { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? EmailId { get; set; }
    public string? EmailAddress { get; set; }
    public string? ProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountKiroDto
{
    public int Id { get; set; }
    public LinkedAccountType LinkedType { get; set; }
    public int RefId { get; set; }
    public string LinkedDisplay { get; set; } = string.Empty;
    public bool IsNew { get; set; }
    public DateTime? LastUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}

