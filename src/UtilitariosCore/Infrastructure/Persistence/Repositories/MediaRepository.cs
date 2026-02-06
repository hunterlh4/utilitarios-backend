using Dapper;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class MediaRepository(MssqlContext context) : IMediaRepository
{
    public async Task<int> CreateMedia(Media item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Media (Type, RefId, Url, Thumbnail, DeleteUrl, OrderIndex, CreatedAt)
        VALUES (@Type, @RefId, @Url, @Thumbnail, @DeleteUrl, @OrderIndex, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> DeleteMedia(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Media WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Media?> GetMediaById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Type, RefId, Url, Thumbnail, DeleteUrl, OrderIndex, CreatedAt FROM Media WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Media>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<Media>> GetMediaByRefId(int refId, MediaType type)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Type, RefId, Url, Thumbnail, DeleteUrl, OrderIndex, CreatedAt FROM Media WHERE RefId = @RefId AND Type = @Type ORDER BY OrderIndex";
        var result = await db.QueryAsync<Media>(sql, new { RefId = refId, Type = type });
        return result;
    }

    public async Task<bool> UpdateMediaOrder(int id, int newOrderIndex)
    {
        var db = context.CreateDefaultConnection();
        string sql = "UPDATE Media SET OrderIndex = @OrderIndex WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id, OrderIndex = newOrderIndex });
        return result > 0;
    }
}
