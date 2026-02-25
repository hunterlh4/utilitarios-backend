using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class HentaiRepository(MssqlContext context) : IHentaiRepository
{
    public async Task<int> CreateHentai(Hentai item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Hentai (ApiId, Title, Image, Episodes, Status, CreatedAt)
        VALUES (@ApiId, @Title, @Image, @Episodes, @Status, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateHentai(Hentai item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Hentai
        SET Title = @Title, Image = @Image, Episodes = @Episodes, Status = @Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteHentai(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Hentai WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Hentai?> GetHentaiById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Hentai WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Hentai>(sql, new { Id = id });
        return result;
    }

    public async Task<Hentai?> GetHentaiByApiId(string apiId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Hentai WHERE ApiId = @ApiId";
        var result = await db.QueryFirstOrDefaultAsync<Hentai>(sql, new { ApiId = apiId });
        return result;
    }

    public async Task<IEnumerable<Hentai>> GetAllHentais()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Hentai ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<Hentai>(sql);
        return result;
    }

    public async Task<HentaiWithTags?> GetHentaiWithTagsById(int id)
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"
            SELECT h.Id, h.ApiId, h.Title, h.Image, h.Episodes, h.Status, h.CreatedAt,
                   t.Name AS TagName
            FROM Hentai h
            LEFT JOIN TagRelation tr ON tr.RefId = h.Id AND tr.Type = 6
            LEFT JOIN Tag t ON t.Id = tr.TagId
            WHERE h.Id = @Id";

        Hentai? hentai = null;
        var tags = new List<string>();

        await db.QueryAsync<Hentai, string?, Hentai>(
            sql,
            (h, tagName) =>
            {
                hentai ??= h;
                if (tagName != null) tags.Add(tagName);
                return h;
            },
            new { Id = id },
            splitOn: "TagName");

        if (hentai is null) return null;
        return new HentaiWithTags { Hentai = hentai, Tags = tags };
    }

    public async Task<IEnumerable<HentaiWithTags>> GetAllHentaisWithTags()
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"
            SELECT h.Id, h.ApiId, h.Title, h.Image, h.Episodes, h.Status, h.CreatedAt,
                   t.Name AS TagName
            FROM Hentai h
            LEFT JOIN TagRelation tr ON tr.RefId = h.Id AND tr.Type = 6
            LEFT JOIN Tag t ON t.Id = tr.TagId
            ORDER BY h.CreatedAt DESC";

        var dict = new Dictionary<int, HentaiWithTags>();

        await db.QueryAsync<Hentai, string?, Hentai>(
            sql,
            (h, tagName) =>
            {
                if (!dict.TryGetValue(h.Id, out var entry))
                {
                    entry = new HentaiWithTags { Hentai = h };
                    dict[h.Id] = entry;
                }
                if (tagName != null) entry.Tags.Add(tagName);
                return h;
            },
            splitOn: "TagName");

        return dict.Values;
    }
}
