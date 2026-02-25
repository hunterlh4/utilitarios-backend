using Dapper;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressJavRepository(MssqlContext context) : IActressJavRepository
{
    public async Task<int> CreateActress(ActressJav actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Actress (Name, Image, CreatedAt)
        VALUES (@Name, @Image, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, actress);
        return result;
    }

    public async Task<bool> UpdateActress(ActressJav actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Actress
        SET Name = @Name, Image = @Image
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, actress);
        return result > 0;
    }

    public async Task<ActressJav?> GetActressById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<ActressJav>(sql, new { Id = id });
        return result;
    }

    public async Task<ActressJav?> GetActressByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<ActressJav>(sql, new { Name = name });
        return result;
    }

    public async Task<IEnumerable<ActressJav>> GetAllActresses()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress ORDER BY Name";
        var result = await db.QueryAsync<ActressJav>(sql);
        return result;
    }

    public async Task<ActressJavWithTagsDto?> GetActressWithTagsById(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT
            a.Id,
            a.Name,
            a.Image,
            a.CreatedAt,
            (
                SELECT STRING_AGG(t.Name, ',')
                FROM TagRelation tr
                INNER JOIN Tag t ON t.Id = tr.TagId
                WHERE tr.RefId = a.Id AND tr.Type = {(int)TagType.ActressJav}
            ) AS TagsRaw
        FROM Actress a
        WHERE a.Id = @Id
        ";

        var row = await db.QueryFirstOrDefaultAsync<ActressJavRawWithTagsDto>(sql, new { Id = id });
        if (row is null) return null;

        return new ActressJavWithTagsDto
        {
            Id = row.Id,
            Name = row.Name,
            Image = row.Image,
            CreatedAt = row.CreatedAt,
            Tags = string.IsNullOrEmpty(row.TagsRaw)
                ? []
                : [.. row.TagsRaw.Split(',')]
        };
    }

    public async Task<IEnumerable<ActressJavDto>> GetAllActressesWithFirstImage()
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT
            a.Id,
            a.Name,
            a.CreatedAt,
            (
                SELECT TOP 1 m.Url
                FROM Media m
                WHERE m.Type = {(int)MediaType.ActressJav}
                AND m.RefId = a.Id
                ORDER BY m.OrderIndex
            ) AS Image,
            (
                SELECT STRING_AGG(t.Name, ',')
                FROM TagRelation tr
                INNER JOIN Tag t ON t.Id = tr.TagId
                WHERE tr.RefId = a.Id AND tr.Type = {(int)TagType.ActressJav}
            ) AS TagsRaw
        FROM Actress a
        ORDER BY a.Name
        ";

        var rows = await db.QueryAsync<ActressJavRawDto>(sql);
        return rows.Select(r => new ActressJavDto
        {
            Id = r.Id,
            Name = r.Name,
            CreatedAt = r.CreatedAt,
            Image = r.Image,
            Tags = string.IsNullOrEmpty(r.TagsRaw)
                ? []
                : [.. r.TagsRaw.Split(',')]
        });
    }
}

