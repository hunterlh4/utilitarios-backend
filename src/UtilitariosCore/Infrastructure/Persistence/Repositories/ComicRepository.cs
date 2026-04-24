using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ComicRepository(MssqlContext context) : IComicRepository
{
    public async Task<int> CreateComic(Comic item)
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"
            INSERT INTO Comic (Name, Image, Url, Category, CreatedAt)
            VALUES (@Name, @Image, @Url, @Category, @CreatedAt)
            SELECT SCOPE_IDENTITY()";
        return await db.QuerySingleAsync<int>(sql, item);
    }

    public async Task<bool> UpdateComic(Comic item)
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"
            UPDATE Comic SET Name = @Name, Image = @Image, Url = @Url, Category = @Category
            WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteComic(int id)
    {
        var db = context.CreateDefaultConnection();
        var result = await db.ExecuteAsync("DELETE FROM Comic WHERE Id = @Id", new { Id = id });
        return result > 0;
    }

    public async Task<Comic> GetComicById(int id)
    {
        var db = context.CreateDefaultConnection();
        const string sql = @"SELECT Id, Name, Image, Url, Category, CreatedAt FROM Comic WHERE Id = @Id";
        return await db.QueryFirstOrDefaultAsync<Comic>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Comic>> GetAllComics()
    {
        var db = context.CreateDefaultConnection();
        const string sql = "SELECT Id, Name, Image, Url, Category, CreatedAt FROM Comic ORDER BY CreatedAt DESC";
        return await db.QueryAsync<Comic>(sql);
    }
}
