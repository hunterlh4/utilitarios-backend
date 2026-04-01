namespace UtilitariosCore.Application.Features.Accounts.Requests;

public class AccountPropertyItem
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class AccountRenewalItem
{
    public int Day { get; set; }
}
