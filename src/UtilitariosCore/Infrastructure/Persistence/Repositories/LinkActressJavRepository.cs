using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class LinkActressJavRepository(MssqlContext context) : ILinkActressJavRepository
{
    public async Task<int> CreateLinkActressJav(LinkActressJav linkActressJav)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO LinkActressJav (ActressJavId, Url, OrderIndex, CreatedAt)
        VALUES (@ActressJavId, @Url, @OrderIndex, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, linkActressJav);
        return result;
    }

    public async Task<List<LinkActressJav>> GetLinkActressJavsByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();
        
        string sql = @"
        SELECT Id, ActressJavId, Url, OrderIndex, CreatedAt 
        FROM LinkActressJav 
        WHERE ActressJavId = @ActressId 
        ORDER BY OrderIndex, Id
        ";

        var result = await db.QueryAsync<LinkActressJav>(sql, new { ActressId = actressId });
        return result.ToList();
    }

    public async Task<bool> DeleteLinkActressJavsByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM LinkActressJav WHERE ActressJavId = @ActressId";
        var affectedRows = await db.ExecuteAsync(sql, new { ActressId = actressId });
        return affectedRows > 0;
    }

    public async Task<LinkActressJav?> GetLinkActressJavById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ActressJavId, Url, OrderIndex, CreatedAt FROM LinkActressJav WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<LinkActressJav>(sql, new { Id = id });
        return result;
    }

    public async Task<bool> UpdateLinkActressJav(LinkActressJav linkActressJav)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE LinkActressJav
        SET Url = @Url, OrderIndex = @OrderIndex
        WHERE Id = @Id
        ";

        var affectedRows = await db.ExecuteAsync(sql, linkActressJav);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteLinkActressJav(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM LinkActressJav WHERE Id = @Id";
        var affectedRows = await db.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}