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

    public async Task<bool> ReplaceTagsForRefId(int refId, TagType type, List<string> tagNames)
    {
        var db = context.CreateDefaultConnection();

        var incoming = tagNames
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Tags actuales ligados a esta entidad
        var current = (await GetTagsByRefId(refId, type))
            .Select(t => t.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Eliminar los que ya no vienen
        var toRemove = current.Except(incoming).ToList();
        foreach (var name in toRemove)
        {
            await db.ExecuteAsync(@"
                DELETE tr FROM TagRelation tr
                INNER JOIN Tag t ON t.Id = tr.TagId
                WHERE tr.RefId = @RefId AND tr.Type = @Type AND t.Name = @Name",
                new { RefId = refId, Type = type, Name = name });
        }

        // Agregar los que son nuevos
        var toAdd = incoming.Except(current).ToList();
        foreach (var name in toAdd)
        {
            // Crear el tag si no existe
            await db.ExecuteAsync(@"
                IF NOT EXISTS (SELECT 1 FROM Tag WHERE Name = @Name AND Type = @Type)
                    INSERT INTO Tag (Name, Type) VALUES (@Name, @Type)",
                new { Name = name, Type = type });

            var tagId = await db.QuerySingleAsync<int>(
                "SELECT Id FROM Tag WHERE Name = @Name AND Type = @Type",
                new { Name = name, Type = type });

            await db.ExecuteAsync(
                "INSERT INTO TagRelation (TagId, RefId, Type) VALUES (@TagId, @RefId, @Type)",
                new { TagId = tagId, RefId = refId, Type = type });
        }

        return true;
    }
}
