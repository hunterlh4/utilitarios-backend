using Dapper;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressAdultRepository(MssqlContext context) : IActressAdultRepository
{
    public async Task<int> CreateActressAdult(ActressAdult actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO ActressAdult (Name, Image, CreatedAt)
        VALUES (@Name, @Image, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";

        var result = await db.QuerySingleAsync<int>(sql, actress);
        return result;
    }

    public async Task<bool> UpdateActressAdult(ActressAdult actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE ActressAdult
        SET Name = @Name
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, actress);
        return result > 0;
    }

    public async Task<ActressAdult?> GetActressAdultById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM ActressAdult WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<ActressAdult>(sql, new { Id = id });
        return result;
    }

    public async Task<ActressAdult?> GetActressAdultByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM ActressAdult WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<ActressAdult>(sql, new { Name = name });
        return result;
    }

    public async Task<bool> UpdateActressAdultImage(int id, string imageUrl)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE ActressAdult
        SET Image = @Image
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, new { Id = id, Image = imageUrl });
        return result > 0;
    }

    public async Task<bool> CheckActressNameExists(string canonicalForm)
    {
        var db = context.CreateDefaultConnection();
        // Obtiene todos los nombres y compara usando forma canónica
        string sql = "SELECT Name FROM ActressAdult";
        var names = await db.QueryAsync<string>(sql);
        
        return names.Any(n => StringNormalizer.GetCanonicalFormForComparison(n).Equals(canonicalForm, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ActressAdultDto?> GetActressAdultWithTagsAndImageById(int id)
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
                WHERE tr.RefId = a.Id AND tr.Type = {(int)TagType.ActressAdult}
            ) AS TagsRaw
        FROM ActressAdult a
        WHERE a.Id = @Id
        ";

        var row = await db.QueryFirstOrDefaultAsync<ActressAdultRawDto>(sql, new { Id = id });
        if (row is null) return null;

        return new ActressAdultDto
        {
            Id = row.Id,
            Name = row.Name,
            CreatedAt = row.CreatedAt,
            Image = row.Image,
            Tags = string.IsNullOrEmpty(row.TagsRaw)
                ? []
                : [.. row.TagsRaw.Split(',')]
        };
    }

    public async Task<IEnumerable<ActressAdultDto>> GetAllActressAdultsWithFirstImage()
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
                WHERE tr.RefId = a.Id AND tr.Type = {(int)TagType.ActressAdult}
            ) AS TagsRaw
        FROM ActressAdult a
        ORDER BY a.Name
        ";

        var rows = await db.QueryAsync<ActressAdultRawDto>(sql);
        return rows.Select(r => new ActressAdultDto
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
