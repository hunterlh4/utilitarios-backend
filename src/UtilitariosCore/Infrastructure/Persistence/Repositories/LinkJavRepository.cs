using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class LinkJavRepository(MssqlContext context) : ILinkJavRepository
{
    public async Task<int> CreateLinkJav(LinkJav linkJav)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO LinkJav (JavId, Url, OrderIndex, CreatedAt)
        VALUES (@JavId, @Url, @OrderIndex, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, linkJav);
        return result;
    }

    public async Task<List<LinkJav>> GetLinkJavsByJavId(int javId)
    {
        var db = context.CreateDefaultConnection();
        
        string sql = @"
        SELECT Id, JavId, Url, OrderIndex, CreatedAt 
        FROM LinkJav 
        WHERE JavId = @JavId 
        ORDER BY OrderIndex, Id
        ";

        var result = await db.QueryAsync<LinkJav>(sql, new { JavId = javId });
        return result.ToList();
    }

    public async Task<bool> DeleteLinkJavsByJavId(int javId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM LinkJav WHERE JavId = @JavId";
        var affectedRows = await db.ExecuteAsync(sql, new { JavId = javId });
        return affectedRows > 0;
    }

    public async Task<LinkJav?> GetLinkJavById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, JavId, Url, OrderIndex, CreatedAt FROM LinkJav WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<LinkJav>(sql, new { Id = id });
        return result;
    }

    public async Task<bool> UpdateLinkJav(LinkJav linkJav)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE LinkJav
        SET Url = @Url, OrderIndex = @OrderIndex
        WHERE Id = @Id
        ";

        var affectedRows = await db.ExecuteAsync(sql, linkJav);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteLinkJav(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM LinkJav WHERE Id = @Id";
        var affectedRows = await db.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}