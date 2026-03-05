using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class SteamItemRepository(MssqlContext context) : ISteamItemRepository
{
    public async Task<IEnumerable<SteamItem>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<SteamItem>(
            "SELECT Id, Name, Image, Price, Game, MarketUrl, Status, CreatedAt FROM SteamItem ORDER BY CreatedAt DESC");
    }

    public async Task<SteamItem?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<SteamItem>(
            "SELECT Id, Name, Image, Price, Game, MarketUrl, Status, CreatedAt FROM SteamItem WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> Create(SteamItem item)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO SteamItem (Name, Image, Price, Game, MarketUrl, Status, CreatedAt)
            VALUES (@Name, @Image, @Price, @Game, @MarketUrl, @Status, @CreatedAt);
            SELECT SCOPE_IDENTITY();", item);
    }

    public async Task<bool> Update(SteamItem item)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE SteamItem SET Name = @Name, Image = @Image, Price = @Price,
            Game = @Game, MarketUrl = @MarketUrl, Status = @Status WHERE Id = @Id", item);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM SteamItem WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM SteamItem WHERE Id = @Id", new { Id = id }) > 0;
    }
}
