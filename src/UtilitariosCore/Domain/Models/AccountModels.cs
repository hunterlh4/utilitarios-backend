using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class AccountEmail
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? RecoveryEmailId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountSteam
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfileUrl { get; set; }
    public bool HasDota2 { get; set; }
    public bool HasCS2 { get; set; }
    public bool IsUnlimited { get; set; }
    public bool IsVacBanned { get; set; }
    public bool HasSteamMobile { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountGitHub
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountGeneral
{
    public int Id { get; set; }
    public GeneralPlatform Platform { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? EmailId { get; set; }
    public string? ProfileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountKiro
{
    public int Id { get; set; }
    public LinkedAccountType LinkedType { get; set; }
    public int RefId { get; set; }
    public bool IsNew { get; set; } = true;
    public DateTime? LastUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}
