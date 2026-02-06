using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class AnimeRepository(MssqlContext context) : IAnimeRepository
{
    public async Task<int> CreateAnime(Anime item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Anime (ApiId, Title, Image, Episodes, Status, CreatedAt)
        VALUES (@ApiId, @Title, @Image, @Episodes, @Status, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateAnime(Anime item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Anime
        SET Title = @Title, Image = @Image, Episodes = @Episodes, Status = @Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteAnime(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = "DELETE FROM Anime WHERE Id = @Id";

        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Anime?> GetAnimeById(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Anime WHERE Id = @Id";

        var result = await db.QueryFirstOrDefaultAsync<Anime>(sql, new { Id = id });
        return result;
    }

    public async Task<Anime?> GetAnimeByApiId(string apiId)
    {
        var db = context.CreateDefaultConnection();

        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Anime WHERE ApiId = @ApiId";

        var result = await db.QueryFirstOrDefaultAsync<Anime>(sql, new { ApiId = apiId });
        return result;
    }

    public async Task<IEnumerable<Anime>> GetAllAnimes()
    {
        var db = context.CreateDefaultConnection();

        string sql = "SELECT Id, ApiId, Title, Image, Episodes, Status, CreatedAt FROM Anime ORDER BY CreatedAt DESC";

        var result = await db.QueryAsync<Anime>(sql);
        return result;
    }
}
