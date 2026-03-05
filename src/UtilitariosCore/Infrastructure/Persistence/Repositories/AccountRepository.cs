using Dapper;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class AccountRepository(MssqlContext context) : IAccountRepository
{
    public async Task<IEnumerable<AccountDto>> GetAll(AccountType? type = null)
    {
        var db = context.CreateDefaultConnection();

        string whereClause = type.HasValue ? "WHERE a.Type = @Type" : string.Empty;

        // Query 1: cuentas principales
        string sqlAccounts = $@"
        SELECT
            Id, Type, Name, Username, Password, ProfileUrl,
            PhoneNumber, RecoveryEmail, LastConnection, CreatedAt
        FROM Account a
        {whereClause}
        ORDER BY a.CreatedAt DESC";

        var accounts = (await db.QueryAsync<AccountDto>(sqlAccounts, new { Type = type })).ToList();

        if (accounts.Count == 0) return accounts;

        var accountIds = accounts.Select(a => a.Id).ToList();

        // Query 2: propiedades
        var properties = (await db.QueryAsync<AccountPropertyDto>(
            "SELECT Id, AccountId, [Key], Value FROM AccountProperty WHERE AccountId IN @Ids",
            new { Ids = accountIds })).ToList();

        // Query 3: renovaciones
        var renewals = (await db.QueryAsync<AccountRenewalDto>(
            "SELECT Id, AccountId, Day, Month, Year FROM AccountRenewal WHERE AccountId IN @Ids ORDER BY Year, Month, Day",
            new { Ids = accountIds })).ToList();

        var propsByAccount = properties.GroupBy(p => p.AccountId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var renewalsByAccount = renewals.GroupBy(r => r.AccountId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var account in accounts)
        {
            account.Properties = propsByAccount.TryGetValue(account.Id, out var props) ? props : [];
            account.Renewals = renewalsByAccount.TryGetValue(account.Id, out var ren) ? ren : [];
        }

        return accounts;
    }

    public async Task<AccountDto?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();

        var account = await db.QueryFirstOrDefaultAsync<AccountDto>(
            @"SELECT Id, Type, Name, Username, Password, ProfileUrl,
              PhoneNumber, RecoveryEmail, LastConnection, CreatedAt
              FROM Account WHERE Id = @Id",
            new { Id = id });

        if (account is null) return null;

        account.Properties = (await db.QueryAsync<AccountPropertyDto>(
            "SELECT Id, AccountId, [Key], Value FROM AccountProperty WHERE AccountId = @Id",
            new { Id = id })).ToList();

        account.Renewals = (await db.QueryAsync<AccountRenewalDto>(
            "SELECT Id, AccountId, Day, Month, Year FROM AccountRenewal WHERE AccountId = @Id ORDER BY Year, Month, Day",
            new { Id = id })).ToList();

        return account;
    }

    public async Task<int> Create(Account account, List<AccountProperty> properties, List<AccountRenewal> renewals)
    {
        var db = context.CreateDefaultConnection();

        int accountId = await db.QuerySingleAsync<int>(@"
        INSERT INTO Account (Type, Name, Username, Password, ProfileUrl, PhoneNumber, RecoveryEmail, LastConnection, CreatedAt)
        VALUES (@Type, @Name, @Username, @Password, @ProfileUrl, @PhoneNumber, @RecoveryEmail, @LastConnection, @CreatedAt);
        SELECT SCOPE_IDENTITY();",
        new
        {
            account.Type,
            account.Name,
            account.Username,
            account.Password,
            account.ProfileUrl,
            account.PhoneNumber,
            account.RecoveryEmail,
            account.LastConnection,
            account.CreatedAt
        });

        foreach (var prop in properties)
        {
            await db.ExecuteAsync(
                "INSERT INTO AccountProperty (AccountId, [Key], Value) VALUES (@AccountId, @Key, @Value)",
                new { AccountId = accountId, prop.Key, prop.Value });
        }

        foreach (var renewal in renewals)
        {
            await db.ExecuteAsync(
                "INSERT INTO AccountRenewal (AccountId, Day, Month, Year) VALUES (@AccountId, @Day, @Month, @Year)",
                new { AccountId = accountId, renewal.Day, renewal.Month, renewal.Year });
        }

        return accountId;
    }

    public async Task<bool> Update(Account account, List<AccountProperty> properties, List<AccountRenewal> renewals)
    {
        var db = context.CreateDefaultConnection();

        int rows = await db.ExecuteAsync(@"
        UPDATE Account SET
            Type = @Type, Name = @Name, Username = @Username, Password = @Password,
            ProfileUrl = @ProfileUrl, PhoneNumber = @PhoneNumber, RecoveryEmail = @RecoveryEmail,
            LastConnection = @LastConnection
        WHERE Id = @Id",
        new
        {
            account.Id,
            account.Type,
            account.Name,
            account.Username,
            account.Password,
            account.ProfileUrl,
            account.PhoneNumber,
            account.RecoveryEmail,
            account.LastConnection
        });

        if (rows == 0) return false;

        // Reemplazar propiedades y renovaciones
        await db.ExecuteAsync("DELETE FROM AccountProperty WHERE AccountId = @Id", new { account.Id });
        await db.ExecuteAsync("DELETE FROM AccountRenewal WHERE AccountId = @Id", new { account.Id });

        foreach (var prop in properties)
        {
            await db.ExecuteAsync(
                "INSERT INTO AccountProperty (AccountId, [Key], Value) VALUES (@AccountId, @Key, @Value)",
                new { AccountId = account.Id, prop.Key, prop.Value });
        }

        foreach (var renewal in renewals)
        {
            await db.ExecuteAsync(
                "INSERT INTO AccountRenewal (AccountId, Day, Month, Year) VALUES (@AccountId, @Day, @Month, @Year)",
                new { AccountId = account.Id, renewal.Day, renewal.Month, renewal.Year });
        }

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        await db.ExecuteAsync("DELETE FROM AccountProperty WHERE AccountId = @Id", new { Id = id });
        await db.ExecuteAsync("DELETE FROM AccountRenewal WHERE AccountId = @Id", new { Id = id });
        int rows = await db.ExecuteAsync("DELETE FROM Account WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>("SELECT COUNT(1) FROM Account WHERE Id = @Id", new { Id = id }) > 0;
    }
}
