using Dapper;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class TagRepository(MssqlContext context) : ITagRepository
{
    public async Task<IEnumerable<Tag>> GetTagsByRefId(int refId, TagType type)
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"
            SELECT t.Id, t.Name, t.Type
            FROM Tag t
            INNER JOIN TagRelation tr ON t.Id = tr.TagId
            WHERE tr.RefId = @RefId AND tr.Type = @Type";
        return await db.QueryAsync<Tag>(sql, new { RefId = refId, Type = type });
    }

    public async Task<IEnumerable<Tag>> GetAllTagsByType(TagType type)
    {
        var db = context.CreateDefaultConnection();
        const string sql = "SELECT Id, Name, Type FROM Tag WHERE Type = @Type ORDER BY Name";
        return await db.QueryAsync<Tag>(sql, new { Type = type });
    }

    public async Task<bool> ReplaceTagsForRefId(int refId, TagType type, List<int> tagIds)
    {
        var db = context.CreateDefaultConnection();

        var incoming = tagIds.ToHashSet();

        // Tags actuales ligados a esta entidad
        var current = (await GetTagsByRefId(refId, type))
            .Select(t => t.Id)
            .ToHashSet();

        // Eliminar los que ya no vienen
        var toRemove = current.Except(incoming).ToList();
        foreach (var id in toRemove)
        {
            await db.ExecuteAsync(
                "DELETE FROM TagRelation WHERE RefId = @RefId AND Type = @Type AND TagId = @TagId",
                new { RefId = refId, Type = type, TagId = id });
        }

        // Agregar los que son nuevos
        var toAdd = incoming.Except(current).ToList();
        foreach (var id in toAdd)
        {
            await db.ExecuteAsync(
                "INSERT INTO TagRelation (TagId, RefId, Type) VALUES (@TagId, @RefId, @Type)",
                new { TagId = id, RefId = refId, Type = type });
        }

        return true;
    }

    public async Task<Tag?> GetTagById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<Tag>(
            "SELECT Id, Name, Type FROM Tag WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> CreateTag(Tag tag)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO Tag (Name, Type) VALUES (@Name, @Type);
            SELECT SCOPE_IDENTITY()", tag);
    }

    public async Task<bool> UpdateTag(Tag tag)
    {
        var db = context.CreateDefaultConnection();
        var rows = await db.ExecuteAsync(
            "UPDATE Tag SET Name = @Name WHERE Id = @Id", tag);
        return rows > 0;
    }

    public async Task<bool> IsTagInUse(int id)
    {
        var db = context.CreateDefaultConnection();
        var count = await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM TagRelation WHERE TagId = @Id", new { Id = id });
        return count > 0;
    }

    public async Task<bool> DeleteTag(int id)
    {
        var db = context.CreateDefaultConnection();
        var rows = await db.ExecuteAsync(
            "DELETE FROM Tag WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }
}
