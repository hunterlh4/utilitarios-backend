using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Account
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
}

public class AccountRelation
{
    public int Id { get; set; }
    public int ParentAccountId { get; set; }
    public int ChildAccountId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountProperty
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public AccountPropertyKey Key { get; set; }
    public bool Value { get; set; }
    public DateTime CreatedAt { get; set; }
}
