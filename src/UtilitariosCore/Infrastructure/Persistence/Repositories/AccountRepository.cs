using Dapper;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class AccountRepository(MssqlContext context) : IAccountRepository
{

    #region account-email
    public async Task<IEnumerable<AccountEmailDto>> GetEmails()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT 
            e.Id, 
            e.Provider,
            e.Email, 
            e.Password, 
            e.Phone,
            e.RecoveryEmailId, 
            r.Email AS RecoveryEmail, 
            e.CreatedAt
            FROM AccountEmail e
            LEFT JOIN AccountEmail r ON r.Id = e.RecoveryEmailId
            ORDER BY e.CreatedAt DESC"; 
        return await db.QueryAsync<AccountEmailDto>(sql);
    }

    public async Task<int> CreateEmail(AccountEmail accountEmail)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
            INSERT INTO AccountEmail (Provider, Email, Password, Phone, RecoveryEmailId, CreatedAt)
            VALUES (@Provider, @Email, @Password, @Phone, @RecoveryEmailId, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)
            ";
        return await db.QuerySingleAsync<int>(sql, accountEmail);
    }

    public async Task<bool> UpdateEmail(AccountEmail accountEmail)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE AccountEmail 
            SET 
            Provider=@Provider, 
            Email=@Email, 
            Password=@Password, 
            Phone=@Phone, 
            RecoveryEmailId=@RecoveryEmailId 
            WHERE Id=@Id";
        return await db.ExecuteAsync(sql, accountEmail) > 0;
    }

    public async Task<bool> DeleteEmail(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AccountEmail WHERE Id=@Id";
        return await db.ExecuteAsync(sql, new { Id = id }) > 0;
    }
    #endregion

    #region account-steam
    public async Task<IEnumerable<AccountSteamDto>> GetSteams()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT s.Id, s.EmailId, e.Email AS EmailAddress, s.Username, s.Password,
                   s.Phone, s.ProfileUrl, s.HasDota2, s.HasCS2, s.IsUnlimited, s.IsVacBanned, s.HasSteamMobile, s.LastPurchaseDate, s.CreatedAt
            FROM AccountSteam s
            LEFT JOIN AccountEmail e ON e.Id = s.EmailId
            ORDER BY s.CreatedAt DESC";
        return await db.QueryAsync<AccountSteamDto>(sql);
    }
    public async Task<int> CreateSteam(AccountSteam accountSteam)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO AccountSteam (EmailId, Username, Password, Phone, ProfileUrl, HasDota2, HasCS2, IsUnlimited, IsVacBanned, HasSteamMobile, LastPurchaseDate, CreatedAt)
            VALUES (@EmailId, @Username, @Password, @Phone, @ProfileUrl, @HasDota2, @HasCS2, @IsUnlimited, @IsVacBanned, @HasSteamMobile, @LastPurchaseDate, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)
            ";
        return await db.QuerySingleAsync<int>(sql, accountSteam);
    }
    public async Task<bool> UpdateSteam(AccountSteam accountSteam)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
             UPDATE AccountSteam SET EmailId=@EmailId, Username=@Username, Password=@Password, Phone=@Phone,
             ProfileUrl=@ProfileUrl, HasDota2=@HasDota2, HasCS2=@HasCS2, IsUnlimited=@IsUnlimited, IsVacBanned=@IsVacBanned, HasSteamMobile=@HasSteamMobile, LastPurchaseDate=@LastPurchaseDate
             WHERE Id=@Id"; 
        return await db.ExecuteAsync(sql, accountSteam) > 0;
    }
    public async Task<bool> DeleteSteam(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AccountSteam WHERE Id=@Id";
        return await db.ExecuteAsync(sql, new { Id = id }) > 0;
    }
    #endregion

    #region account-github
    public async Task<IEnumerable<AccountGitHubDto>> GetGitHubs()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT g.Id, g.EmailId, e.Email AS EmailAddress, g.Username, g.Password, g.ProfileUrl, g.CreatedAt
            FROM AccountGitHub g
            LEFT JOIN AccountEmail e ON e.Id = g.EmailId
            ORDER BY g.CreatedAt DESC";
        return await db.QueryAsync<AccountGitHubDto>(sql);
    }
    public async Task<int> CreateGitHub(AccountGitHub accountGitHub)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO AccountGitHub (EmailId, Username, Password, ProfileUrl, CreatedAt)
            VALUES (@EmailId, @Username, @Password, @ProfileUrl, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)
            ";
        return await db.QuerySingleAsync<int>(sql, accountGitHub);
    }

    public async Task<bool> UpdateGitHub(AccountGitHub accountGitHub)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
             UPDATE AccountGitHub SET EmailId=@EmailId, Username=@Username, Password=@Password, ProfileUrl=@ProfileUrl
             WHERE Id=@Id";
        return await db.ExecuteAsync(sql, accountGitHub) > 0;
    }
    public async Task<bool> DeleteGitHub(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AccountGitHub WHERE Id=@Id";
        return await db.ExecuteAsync(sql, new { Id = id }) > 0;
    }
    #endregion
    
    #region account-general
    public async Task<IEnumerable<AccountGeneralDto>> GetGenerals()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT g.Id, g.Platform, g.Username, g.Password, g.EmailId, e.Email AS EmailAddress, g.ProfileUrl, g.CreatedAt
            FROM AccountGeneral g
            LEFT JOIN AccountEmail e ON e.Id = g.EmailId
            ORDER BY g.Platform, g.CreatedAt DESC";
        return await db.QueryAsync<AccountGeneralDto>(sql);
    }
    
    public async Task<int> CreateGeneral(AccountGeneral accountGeneral)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO AccountGeneral (Platform, Username, Password, EmailId, ProfileUrl, CreatedAt)
            VALUES (@Platform, @Username, @Password, @EmailId, @ProfileUrl, @CreatedAt);
            SELECT SCOPE_IDENTITY();";
        return await db.QuerySingleAsync<int>(sql, accountGeneral);
    }

    public async Task<bool> UpdateGeneral(AccountGeneral accountGeneral)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE AccountGeneral SET Platform=@Platform, Username=@Username, Password=@Password,
            EmailId=@EmailId, ProfileUrl=@ProfileUrl WHERE Id=@Id";
        return await db.ExecuteAsync(sql, accountGeneral) > 0;
    }

    public async Task<bool> DeleteGeneral(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AccountGeneral WHERE Id=@Id";
        return await db.ExecuteAsync(sql, new { Id = id }) > 0;
    }
    #endregion
    
    #region account-kiro
    public async Task<IEnumerable<AccountKiroDto>> GetKiro()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
            SELECT k.Id, k.LinkedType, k.RefId, k.IsNew, k.LastUsed, k.CreatedAt,
                   CASE k.LinkedType WHEN 1 THEN e.Email WHEN 2 THEN gh.Username END AS LinkedDisplay
            FROM AccountKiro k
            LEFT JOIN AccountEmail e ON k.LinkedType = 1 AND e.Id = k.RefId
            LEFT JOIN AccountGitHub gh ON k.LinkedType = 2 AND gh.Id = k.RefId";

        var result = await db.QueryAsync<AccountKiroDto>(sql);
        return result;
    }

    
    public async Task<int> CreateKiro(AccountKiro accountKiro)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO AccountKiro (LinkedType, RefId, IsNew, LastUsed, CreatedAt)
            VALUES (@LinkedType, @RefId, @IsNew, @LastUsed, @CreatedAt);
            SELECT SCOPE_IDENTITY();";
        return await db.QuerySingleAsync<int>(sql, accountKiro);
    }

    public async Task<bool> UpdateKiro(AccountKiro accountKiro)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE AccountKiro SET LinkedType=@LinkedType, RefId=@RefId, IsNew=@IsNew, LastUsed=@LastUsed WHERE Id=@Id";
        return await db.ExecuteAsync(sql, accountKiro) > 0;
    }

    public async Task<bool> UseKiro(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "UPDATE AccountKiro SET LastUsed=@Now, IsNew=0 WHERE Id=@Id";
        int rows = await db.ExecuteAsync(sql, new { Id = id, Now = DateTime.Now });
        return rows > 0;
    }

    public async Task<int> ResetKiro(DateTime threshold)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE AccountKiro SET LastUsed = NULL
            WHERE IsNew = 0 AND LastUsed IS NOT NULL AND LastUsed < @Threshold";
        return await db.ExecuteAsync(sql, new { Threshold = threshold });
    }
    #endregion


 

}
