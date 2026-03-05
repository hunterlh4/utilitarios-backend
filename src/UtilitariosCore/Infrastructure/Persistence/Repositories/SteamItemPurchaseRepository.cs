using Dapper;
using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class SteamItemPurchaseRepository(MssqlContext context) : ISteamItemPurchaseRepository
{
    private const string SelectWithItem = @"
        SELECT p.Id, p.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
               s.MarketUrl AS ItemMarketUrl, p.PurchasePrice, p.SalePrice, p.Profit,
               p.Status, p.PurchaseDate, p.SaleDate, p.CreatedAt
        FROM SteamItemPurchase p
        INNER JOIN SteamItem s ON s.Id = p.SteamItemId";

    public async Task<IEnumerable<SteamItemPurchaseDto>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<SteamItemPurchaseDto>(SelectWithItem + " ORDER BY p.PurchaseDate DESC");
    }

    public async Task<SteamItemPurchaseDto?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<SteamItemPurchaseDto>(
            SelectWithItem + " WHERE p.Id = @Id", new { Id = id });
    }

    public async Task<int> Create(SteamItemPurchase purchase)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO SteamItemPurchase (SteamItemId, PurchasePrice, SalePrice, Profit, Status, PurchaseDate, SaleDate, CreatedAt)
            VALUES (@SteamItemId, @PurchasePrice, @SalePrice, @Profit, @Status, @PurchaseDate, @SaleDate, @CreatedAt);
            SELECT SCOPE_IDENTITY();", purchase);
    }

    public async Task<bool> Update(SteamItemPurchase purchase)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE SteamItemPurchase SET SteamItemId = @SteamItemId, PurchasePrice = @PurchasePrice,
            SalePrice = @SalePrice, Profit = @Profit, Status = @Status,
            PurchaseDate = @PurchaseDate, SaleDate = @SaleDate WHERE Id = @Id", purchase);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM SteamItemPurchase WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM SteamItemPurchase WHERE Id = @Id", new { Id = id }) > 0;
    }
}
