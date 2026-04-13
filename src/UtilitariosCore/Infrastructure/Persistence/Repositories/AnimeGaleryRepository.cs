using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class GaleryRepository(MssqlContext context) : IGaleryRepository
{
    #region Anime-Galery
    public async Task<int> CreateAnimeGalery(AnimeGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO AnimeGalery (Name, Image, CreatedAt)
        VALUES (@Name, @Image, @CreatedAt)
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
        SET Name = @Name,
            Image = @Image
        WHERE Id = @Id
        ";
        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }
    public async Task<bool> UpdateAnimeGaleryImage(int id, string imageUrl)
    {
        var db = context.CreateDefaultConnection();
        const string sql = "UPDATE AnimeGalery SET Image = @Image WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id, Image = imageUrl });
        return result > 0;
    }

    public async Task<bool> DeleteAnimeGalery(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM AnimeGalery WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<AnimeGalery> GetAnimeGaleryById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM AnimeGalery WHERE Id = @Id";
        return await db.QueryFirstOrDefaultAsync<AnimeGalery>(sql, new { Id = id }); ;
    }

    public async Task<AnimeGalery> GetAnimeGaleryByName(string name)
    {
        var db = context.CreateDefaultConnection();
        const string sql = "SELECT TOP 1 Id, Name, Image, CreatedAt FROM AnimeGalery WHERE Name = @Name ORDER BY Id DESC";
        return await db.QueryFirstOrDefaultAsync<AnimeGalery>(sql, new { Name = name });
    }

    public async Task<IEnumerable<AnimeGalery>> GetAllAnimeGaleries()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM AnimeGalery ORDER BY CreatedAt DESC";
        return await db.QueryAsync<AnimeGalery>(sql); ;
    }
    #endregion
        
    #region girls-galery

     public async Task<int> CreateGirlGalery(GirlGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO GirlGalery (Name, Image, CreatedAt)
        VALUES (@Name, @Image, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateGirlGalery(GirlGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE GirlGalery
        SET Name = @Name,
            Image = @Image
        WHERE Id = @Id
        ";
        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }
    public async Task<bool> UpdateGirlGaleryImage(int id, string imageUrl)
    {
        var db = context.CreateDefaultConnection();
        const string sql = "UPDATE GirlGalery SET Image = @Image WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id, Image = imageUrl });
        return result > 0;
    }

    public async Task<bool> DeleteGirlGalery(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM GirlGalery WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<GirlGalery> GetGirlGaleryById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM GirlGalery WHERE Id = @Id";
        return await db.QueryFirstOrDefaultAsync<GirlGalery>(sql, new { Id = id }); ;
    }

    public async Task<GirlGalery> GetGirlGaleryByName(string name)
    {
        var db = context.CreateDefaultConnection();
        const string sql = "SELECT TOP 1 Id, Name, Image, CreatedAt FROM GirlGalery WHERE Name = @Name ORDER BY Id DESC";
        return await db.QueryFirstOrDefaultAsync<GirlGalery>(sql, new { Name = name });
    }

    public async Task<IEnumerable<GirlGalery>> GetAllGirlGaleries()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM GirlGalery ORDER BY CreatedAt DESC";
        return await db.QueryAsync<GirlGalery>(sql); ;
    }
    #endregion
}
