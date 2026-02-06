using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class AnimeGaleryRepository(MssqlContext context) : IAnimeGaleryRepository
{
    public async Task<int> CreateAnimeGalery(AnimeGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO AnimeGalery (Name, CreatedAt)
        VALUES (@Name, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateAnimeGalery(AnimeGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE AnimeGalery
        SET Name = @Name
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteAnimeGalery(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AnimeGalery WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<AnimeGalery?> GetAnimeGaleryById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM AnimeGalery WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<AnimeGalery>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<AnimeGalery>> GetAllAnimeGaleries()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM AnimeGalery ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<AnimeGalery>(sql);
        return result;
    }
}
