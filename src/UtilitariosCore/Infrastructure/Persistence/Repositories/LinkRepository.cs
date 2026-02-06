using Dapper;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class LinkRepository(MssqlContext context) : ILinkRepository
{
    public async Task<int> CreateLink(Link link)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Link (Type, RefId, Name, Url, OrderIndex, CreatedAt)
        VALUES (@Type, @RefId, @Name, @Url, @OrderIndex, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, link);
        return result;
    }

    public async Task<bool> UpdateLink(Link link)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Link
        SET Name = @Name, Url = @Url, OrderIndex = @OrderIndex
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, link);
        return result > 0;
    }

    public async Task<bool> DeleteLink(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Link WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Link?> GetLinkById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Type, RefId, Name, Url, OrderIndex, CreatedAt FROM Link WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Link>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<Link>> GetLinksByRefId(int refId, LinkType type)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Type, RefId, Name, Url, OrderIndex, CreatedAt FROM Link WHERE RefId = @RefId AND Type = @Type ORDER BY OrderIndex";
        var result = await db.QueryAsync<Link>(sql, new { RefId = refId, Type = type });
        return result;
    }

    public async Task<bool> DeleteLinksByRefId(int refId, LinkType type)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Link WHERE RefId = @RefId AND Type = @Type";
        var result = await db.ExecuteAsync(sql, new { RefId = refId, Type = type });
        return result > 0;
    }
}
