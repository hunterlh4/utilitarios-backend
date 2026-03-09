using Dapper;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressJavRepository(MssqlContext context) : IActressJavRepository
{
    public async Task<int> CreateActressJav(ActressJav actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO ActressJav (Name, CreatedAt)
        VALUES (@Name, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, actress);
        return result;
    }

    public async Task<bool> UpdateActressJav(ActressJav actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE ActressJav
        SET Name = @Name
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, actress);
        return result > 0;
    }

    public async Task<ActressJav?> GetActressJavById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressJav WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<ActressJav>(sql, new { Id = id });
        return result;
    }

    public async Task<ActressJav?> GetActressJavByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressJav WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<ActressJav>(sql, new { Name = name });
        return result;
    }

    public async Task<bool> CheckActressNameExists(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT COUNT(1) FROM ActressJav WHERE LOWER(Name) = LOWER(@Name)";
        var count = await db.ExecuteScalarAsync<int>(sql, new { Name = name });
        return count > 0;
    }

    public async Task<bool> DeleteActressJav(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM ActressJav WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<IEnumerable<ActressJav>> GetAllActressJav()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressJav ORDER BY Name";
        var result = await db.QueryAsync<ActressJav>(sql);
        return result;
    }

    public async Task<ActressJavWithTagsDto?> GetActressJavWithTagsById(int id)
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
        FROM ActressJav a
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

    public async Task<IEnumerable<ActressJavDto>> GetAllActressJavWithFirstImage()
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
            ) AS TagsRaw,
            (
                SELECT COUNT(DISTINCT ja.JavId)
                FROM JavActress ja
                WHERE ja.ActressId = a.Id
            ) AS JavCount
        FROM ActressJav a
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
                : [.. r.TagsRaw.Split(',').Select(t => t.Trim())],
            JavCount = r.JavCount
        });
    }
}

