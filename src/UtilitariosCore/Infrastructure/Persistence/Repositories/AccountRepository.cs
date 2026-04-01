using Dapper;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class AccountRepository(MssqlContext context) : IAccountRepository
{
    public async Task<IEnumerable<AccountEmailDto>> GetEmails()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<AccountEmailDto>(@"
            SELECT e.Id, e.Provider, e.Email, e.Password, e.Phone,
                   e.RecoveryEmailId, r.Email AS RecoveryEmail, e.CreatedAt
            FROM AccountEmail e
            LEFT JOIN AccountEmail r ON r.Id = e.RecoveryEmailId
            ORDER BY e.CreatedAt DESC");
    }

    public async Task<IEnumerable<AccountSteamDto>> GetSteams()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<AccountSteamDto>(@"
            SELECT s.Id, s.EmailId, e.Email AS EmailAddress, s.Username, s.Password,
                   s.Phone, s.ProfileUrl, s.HasDota2, s.HasCS2, s.IsUnlimited, s.IsVacBanned, s.CreatedAt
            FROM AccountSteam s
            LEFT JOIN AccountEmail e ON e.Id = s.EmailId
            ORDER BY s.CreatedAt DESC");
    }

    public async Task<IEnumerable<AccountGitHubDto>> GetGitHubs()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<AccountGitHubDto>(@"
            SELECT g.Id, g.EmailId, e.Email AS EmailAddress, g.Username, g.Password, g.ProfileUrl, g.CreatedAt
            FROM AccountGitHub g
            LEFT JOIN AccountEmail e ON e.Id = g.EmailId
            ORDER BY g.CreatedAt DESC");
    }

    public async Task<IEnumerable<AccountGeneralDto>> GetGenerals()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<AccountGeneralDto>(@"
            SELECT g.Id, g.Platform, g.Username, g.Password, g.EmailId, e.Email AS EmailAddress, g.ProfileUrl, g.CreatedAt
            FROM AccountGeneral g
            LEFT JOIN AccountEmail e ON e.Id = g.EmailId
            ORDER BY g.Platform, g.CreatedAt DESC");
    }

    public async Task<AccountKiroDto?> GetKiro()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<AccountKiroDto>(@"
            SELECT k.Id, k.LinkedType, k.RefId, k.IsNew, k.LastUsed, k.CreatedAt,
                   CASE k.LinkedType WHEN 1 THEN e.Email WHEN 2 THEN gh.Username END AS LinkedDisplay
            FROM AccountKiro k
            LEFT JOIN AccountEmail e ON k.LinkedType = 1 AND e.Id = k.RefId
            LEFT JOIN AccountGitHub gh ON k.LinkedType = 2 AND gh.Id = k.RefId");
    }

    public async Task<int> CreateEmail(AccountEmail a)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO AccountEmail (Provider, Email, Password, Phone, RecoveryEmailId, CreatedAt)
            VALUES (@Provider, @Email, @Password, @Phone, @RecoveryEmailId, @CreatedAt);
            SELECT SCOPE_IDENTITY();", a);
    }

    public async Task<bool> UpdateEmail(AccountEmail a)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync(@"
            UPDATE AccountEmail SET Provider=@Provider, Email=@Email, Password=@Password,
            Phone=@Phone, RecoveryEmailId=@RecoveryEmailId WHERE Id=@Id", a) > 0;
    }

    public async Task<bool> DeleteEmail(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync("DELETE FROM AccountEmail WHERE Id=@Id", new { Id = id }) > 0;
    }

    public async Task<int> CreateSteam(AccountSteam a)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO AccountSteam (EmailId, Username, Password, Phone, ProfileUrl, HasDota2, HasCS2, IsUnlimited, IsVacBanned, CreatedAt)
            VALUES (@EmailId, @Username, @Password, @Phone, @ProfileUrl, @HasDota2, @HasCS2, @IsUnlimited, @IsVacBanned, @CreatedAt);
            SELECT SCOPE_IDENTITY();", a);
    }

    public async Task<bool> UpdateSteam(AccountSteam a)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync(@"
            UPDATE AccountSteam SET EmailId=@EmailId, Username=@Username, Password=@Password, Phone=@Phone,
            ProfileUrl=@ProfileUrl, HasDota2=@HasDota2, HasCS2=@HasCS2, IsUnlimited=@IsUnlimited, IsVacBanned=@IsVacBanned
            WHERE Id=@Id", a) > 0;
    }

    public async Task<bool> DeleteSteam(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync("DELETE FROM AccountSteam WHERE Id=@Id", new { Id = id }) > 0;
    }

    public async Task<int> CreateGitHub(AccountGitHub a)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO AccountGitHub (EmailId, Username, Password, ProfileUrl, CreatedAt)
            VALUES (@EmailId, @Username, @Password, @ProfileUrl, @CreatedAt);
            SELECT SCOPE_IDENTITY();", a);
    }

    public async Task<bool> UpdateGitHub(AccountGitHub a)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync(@"
            UPDATE AccountGitHub SET EmailId=@EmailId, Username=@Username, Password=@Password, ProfileUrl=@ProfileUrl
            WHERE Id=@Id", a) > 0;
    }

    public async Task<bool> DeleteGitHub(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync("DELETE FROM AccountGitHub WHERE Id=@Id", new { Id = id }) > 0;
    }

    public async Task<int> CreateGeneral(AccountGeneral a)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO AccountGeneral (Platform, Username, Password, EmailId, ProfileUrl, CreatedAt)
            VALUES (@Platform, @Username, @Password, @EmailId, @ProfileUrl, @CreatedAt);
            SELECT SCOPE_IDENTITY();", a);
    }

    public async Task<bool> UpdateGeneral(AccountGeneral a)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync(@"
            UPDATE AccountGeneral SET Platform=@Platform, Username=@Username, Password=@Password,
            EmailId=@EmailId, ProfileUrl=@ProfileUrl WHERE Id=@Id", a) > 0;
    }

    public async Task<bool> DeleteGeneral(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync("DELETE FROM AccountGeneral WHERE Id=@Id", new { Id = id }) > 0;
    }

    public async Task<int> CreateKiro(AccountKiro a)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO AccountKiro (LinkedType, RefId, IsNew, LastUsed, CreatedAt)
            VALUES (@LinkedType, @RefId, @IsNew, @LastUsed, @CreatedAt);
            SELECT SCOPE_IDENTITY();", a);
    }

    public async Task<bool> UpdateKiro(AccountKiro a)
    {
        var db = context.CreateDefaultConnection();
        return await db.ExecuteAsync(@"
            UPDATE AccountKiro SET LinkedType=@LinkedType, RefId=@RefId, IsNew=@IsNew, LastUsed=@LastUsed WHERE Id=@Id", a) > 0;
    }

    public async Task<bool> UseKiro(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("UPDATE AccountKiro SET LastUsed=@Now, IsNew=0 WHERE Id=@Id", new { Id = id, Now = DateTime.Now });
        return rows > 0;
    }

    public async Task<int> ResetKiro(DateTime threshold)
    {
        var db = context.CreateDefaultConnection();
        // Limpia LastUsed solo si IsNew=0 y LastUsed < threshold (día 1 del mes actual en hora Perú)
        return await db.ExecuteAsync(@"
            UPDATE AccountKiro SET LastUsed = NULL
            WHERE IsNew = 0 AND LastUsed IS NOT NULL AND LastUsed < @Threshold",
            new { Threshold = threshold });
    }
}
