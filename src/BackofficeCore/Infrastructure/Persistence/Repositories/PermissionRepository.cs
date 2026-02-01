using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Domain.Models;
using Dapper;

namespace BackofficeCore.Infrastructure.Persistence.Repositories;

public class PermissionRepository(MssqlContext context) : IPermissionRepository
{
    public async Task<bool> CreateManyPermissions(IEnumerable<Permission> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Permissions
        (
            Name,
            Controller,
            ActionName,
            HttpMethod,
            ActionType,
            CreatedAt
        )
        VALUES
        (
            @Name,
            @Controller,
            @ActionName,
            @HttpMethod,
            @ActionType,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<bool> UpdateManyPermissions(IEnumerable<Permission> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Permissions
        SET
            Name = @Name,
            Controller = @Controller,
            ActionName = @ActionName,
            HttpMethod = @HttpMethod,
            ActionType = @ActionType,
            UpdatedAt = @UpdatedAt
        WHERE
            Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<bool> DeletePermissionById(int itemId)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM Permissions WHERE Id = @Id";

        var result = await db.ExecuteAsync(sql, new { Id = itemId });

        return result > 0;
    }

    public async Task<Permission?> GetPermissionById(int itemId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Name,
            Controller,
            ActionName,
            HttpMethod,
            ActionType,
            CreatedAt,
            UpdatedAt
        FROM Permissions
        WHERE
            Id = @Id
        ";

        var result = await db.QueryFirstOrDefaultAsync<Permission>(sql, new { Id = itemId });

        return result;
    }

    public async Task<IEnumerable<Permission>> GetAllPermissions()
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            Id,
            Name,
            Controller,
            ActionName,
            HttpMethod,
            ActionType,
            CreatedAt,
            UpdatedAt
        FROM Permissions
        ";

        var result = await db.QueryAsync<Permission>(sql);

        return result;
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByUserId(int userId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT DISTINCT
            p.Id,
            p.Name,
            p.Controller,
            p.ActionName,
            p.HttpMethod,
            p.ActionType,
            p.CreatedAt,
            p.UpdatedAt
        FROM Permissions p
        INNER JOIN RolePermissions rp ON rp.PermissionId = p.Id
        INNER JOIN UserRoles ur ON ur.RoleId = rp.RoleId
        WHERE
            ur.UserId = @UserId
        ";

        var result = await db.QueryAsync<Permission>(sql, new { UserId = userId });

        return result;
    }

    public async Task<bool> CreateRolePermission(RolePermission item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO RolePermissions
        (
            RoleId,
            PermissionId,
            CreatedAt
        )
        VALUES
        (
            @RoleId,
            @PermissionId,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, item);

        return result > 0;
    }

    public async Task<bool> DeleteRolePermission(int roleId, int permissionId)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId";

        var result = await db.ExecuteAsync(sql, new { RoleId = roleId, PermissionId = permissionId });

        return result > 0;
    }

    public async Task<bool> DeleteRolePermissionByRole(int roleId)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM RolePermissions WHERE RoleId = @RoleId";

        var result = await db.ExecuteAsync(sql, new { RoleId = roleId });

        return result > 0;
    }

    public async Task<bool> CreateManyRolePermission(IEnumerable<RolePermission> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO RolePermissions
        (
            RoleId,
            PermissionId,
            CreatedAt
        )
        VALUES
        (
            @RoleId,
            @PermissionId,
            @CreatedAt
        )
        ";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<bool> DeleteManyRolePermission(IEnumerable<RolePermission> items)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId";

        var result = await db.ExecuteAsync(sql, items);

        return result > 0;
    }

    public async Task<RolePermission?> GetRolePermissionByIds(int roleId, int permissionId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            RoleId,
            PermissionId,
            CreatedAt
        FROM RolePermissions
        WHERE
            RoleId = @RoleId AND PermissionId = @PermissionId
        ";

        var result = await db.QueryFirstOrDefaultAsync<RolePermission>(sql, new { RoleId = roleId, PermissionId = permissionId });

        return result;
    }

    public async Task<IEnumerable<RolePermission>> GetPermissionsByRoleId(int roleId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT
            RoleId,
            PermissionId,
            CreatedAt
        FROM RolePermissions
        WHERE
            RoleId = @RoleId
        ";

        var result = await db.QueryAsync<RolePermission>(sql, new { RoleId = roleId });

        return result;
    }
}
