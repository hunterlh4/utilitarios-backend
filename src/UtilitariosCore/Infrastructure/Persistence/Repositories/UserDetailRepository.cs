using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using Dapper;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class UserDetailRepository(MssqlContext context) : IUserDetailRepository
{
    public async Task<UserDetail?> GetUserDetailByUserId(int userId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            UserId,
            FirstName,
            LastName,
            Email,
            PhoneNumber,
            CountryCode,
            CreatedAt,
            UpdatedAt
        FROM UserDetails
        WHERE
            UserId = @UserId
        ";

        var result = await db.QueryFirstOrDefaultAsync<UserDetail>(sql, new { UserId = userId });

        return result;
    }
}
