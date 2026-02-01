using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using Dapper;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class UserRepository(MssqlContext context) : IUserRepository
{
    public async Task<int> CreateUser(User item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Users
        (
            Username,
            PasswordHash,
            UserType,
            SuperUser,
            CreatedAt
        )
        VALUES
        (
            @Username,
            @PasswordHash,
            @UserType,
            @SuperUser,
            @CreatedAt
        )

        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);

        return result;
    }

    public async Task<bool> UpdateUser(User item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Users
        SET
            Username = @Username,
            UserType = @UserType,
            SuperUser = @SuperUser,
            UpdatedAt = @UpdatedAt
        WHERE
            Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<User?> GetUserById(int userId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Username,
            PasswordHash,
            UserType,
            SuperUser,
            CreatedAt,
            UpdatedAt
        FROM Users
        WHERE
            Id = @UserId
        ";

        var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });

        return result;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Username,
            PasswordHash,
            UserType,
            SuperUser,
            CreatedAt,
            UpdatedAt
        FROM Users
        WHERE
            Username = @Username
        ";

        var result = await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });

        return result;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Username,
            PasswordHash,
            UserType,
            SuperUser,
            CreatedAt,
            UpdatedAt
        FROM Users
        ";

        var result = await db.QueryAsync<User>(sql);

        return result;
    }

    public async Task<IEnumerable<User>> GetUsersByRoleId(int roleId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            u.Id,
            u.Username,
            u.PasswordHash,
            u.UserType,
            u.SuperUser,
            u.CreatedAt,
            u.UpdatedAt
        FROM Users u
        INNER JOIN UserRoles ur ON ur.UserId = u.Id
        WHERE
            ur.RoleId = @RoleId
        ";

        var result = await db.QueryAsync<User>(sql, new { RoleId = roleId });

        return result;
    }

    public async Task<bool> UpdatePassword(User item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Users
        SET
            PasswordHash = @PasswordHash,
            UpdatedAt = @UpdatedAt
        WHERE
            Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<bool> CreateUserRole(UserRole item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO UserRoles
        (
            UserId,
            RoleId,
            CreatedAt
        )
        VALUES
        (
            @UserId,
            @RoleId,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<bool> DeleteUserRole(int userId, int roleId)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";

        var result = await db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });

        return result > 0;
    }

    public async Task<UserRole?> GetUserRoleByIds(int userId, int roleId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            UserId,
            RoleId,
            CreatedAt
        FROM UserRoles
        WHERE
            UserId = @UserId AND RoleId = @RoleId
        ";

        var result = await db.QueryFirstOrDefaultAsync<UserRole>(sql, new { UserId = userId, RoleId = roleId });

        return result;
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRoles()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            UserId,
            RoleId,
            CreatedAt
        FROM UserRoles
        ";

        var result = await db.QueryAsync<UserRole>(sql);

        return result;
    }

    public async Task<IEnumerable<UserRole>> GetUserRolesByUserId(int userId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            UserId,
            RoleId,
            CreatedAt
        FROM UserRoles
        WHERE
            UserId = @UserId
        ";

        var result = await db.QueryAsync<UserRole>(sql, new { UserId = userId });

        return result;
    }

    public async Task<bool> CreateUserDetail(UserDetail item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO UserDetails
        (
            UserId,
            FirstName,
            LastName,
            Email,
            PhoneNumber,
            CountryCode,
            CreatedAt
        )
        VALUES
        (
            @UserId,
            @FirstName,
            @LastName,
            @Email,
            @PhoneNumber,
            @CountryCode,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<int> CreateUserWithDetail(User user, UserDetail detail)
    {
        var db = context.CreateDefaultConnection();

        using var transaction = db.BeginTransaction();

        try
        {
            string userSql = @"
            INSERT INTO Users
            (
                Username,
                PasswordHash,
                UserType,
                SuperUser,
                CreatedAt
            )
            VALUES
            (
                @Username,
                @PasswordHash,
                @UserType,
                @SuperUser,
                @CreatedAt
            )

            SELECT SCOPE_IDENTITY()
            ";

            var userId = await db.QuerySingleAsync<int>(userSql, user, transaction);

            detail.UserId = userId;

            string detailSql = @"
            INSERT INTO UserDetails
            (
                UserId,
                FirstName,
                LastName,
                Email,
                PhoneNumber,
                CountryCode,
                CreatedAt
            )
            VALUES
            (
                @UserId,
                @FirstName,
                @LastName,
                @Email,
                @PhoneNumber,
                @CountryCode,
                @CreatedAt
            )
            ";

            await db.ExecuteAsync(detailSql, detail, transaction);

            transaction.Commit();

            return userId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersWithDetails()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            u.Id,
            u.Username,
            u.PasswordHash,
            u.UserType,
            u.SuperUser,
            u.CreatedAt,
            u.UpdatedAt,
            ud.UserId,
            ud.FirstName,
            ud.LastName,
            ud.Email,
            ud.PhoneNumber,
            ud.CountryCode,
            ud.CreatedAt,
            ud.UpdatedAt
        FROM Users u
        LEFT JOIN UserDetails ud ON ud.UserId = u.Id
        ";

        var result = await db.QueryAsync<User, UserDetail, User>(sql, (user, detail) =>
        {
            user.Detail = detail;
            return user;
        },
        splitOn: "UserId");

        return result;
    }

    public async Task<IEnumerable<User>> GetUsersByType(int userType)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Username,
            PasswordHash,
            UserType,
            SuperUser,
            CreatedAt,
            UpdatedAt
        FROM Users
        WHERE
            UserType = @UserType
        ";

        var result = await db.QueryAsync<User>(sql, new { UserType = userType });

        return result;
    }

    public async Task<UserDetail?> GetUserDetailById(int userId)
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

    public async Task<bool> UpdateUserDetail(UserDetail item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE UserDetails
        SET
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            CountryCode = @CountryCode,
            UpdatedAt = @UpdatedAt
        WHERE
            UserId = @UserId
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }
}
