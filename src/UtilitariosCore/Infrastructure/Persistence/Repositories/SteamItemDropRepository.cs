using Dapper;
using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class SteamItemDropRepository(MssqlContext context) : ISteamItemDropRepository
{
    private const string SelectWithItem = @"
        SELECT d.Id, d.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
               s.MarketUrl AS ItemMarketUrl, d.Quantity, d.Price, d.SalePrice, d.Total, d.CreatedAt
        FROM SteamItemDrop d
        INNER JOIN SteamItem s ON s.Id = d.SteamItemId";

    public async Task<IEnumerable<SteamItemDropDto>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<SteamItemDropDto>(SelectWithItem + " ORDER BY d.CreatedAt DESC");
    }

    public async Task<SteamItemDropDto?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<SteamItemDropDto>(
            SelectWithItem + " WHERE d.Id = @Id", new { Id = id });
    }

    public async Task<int> Create(SteamItemDrop drop)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO SteamItemDrop (SteamItemId, Quantity, Price, SalePrice, Total, CreatedAt)
            VALUES (@SteamItemId, @Quantity, @Price, @SalePrice, @Total, @CreatedAt);
            SELECT SCOPE_IDENTITY();", drop);
    }

    public async Task<bool> Update(SteamItemDrop drop)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE SteamItemDrop SET SteamItemId = @SteamItemId, Quantity = @Quantity,
            Price = @Price, SalePrice = @SalePrice, Total = @Total WHERE Id = @Id", drop);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM SteamItemDrop WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM SteamItemDrop WHERE Id = @Id", new { Id = id }) > 0;
    }
}
