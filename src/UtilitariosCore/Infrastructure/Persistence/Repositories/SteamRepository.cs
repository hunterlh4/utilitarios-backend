using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class SteamRepository(MssqlContext context) : ISteamRepository
{
    #region steam-item
    public async Task<IEnumerable<SteamItem>> GetAllItems()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ExternalId, Name, Image, Price, Game, MarketUrl, Status, CreatedAt, UpdatedAt FROM SteamItem ORDER BY CreatedAt DESC";
        return await db.QueryAsync<SteamItem>(sql);
    }

    public async Task<SteamItem> GetByIdItems(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql ="SELECT Id, ExternalId, Name, Image, Price, Game, MarketUrl, Status, CreatedAt, UpdatedAt FROM SteamItem WHERE Id = @Id";
        return await db.QueryFirstOrDefaultAsync<SteamItem>(sql,new { Id = id });
    }

    public async Task<SteamItem?> GetItemByExternalIdAsync(string externalId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ExternalId, Name, Image, Price, Game, MarketUrl, Status, CreatedAt, UpdatedAt FROM SteamItem WHERE ExternalId = @ExternalId";
        return await db.QueryFirstOrDefaultAsync<SteamItem>(sql, new { ExternalId = externalId });
    }

    public async Task<SteamItem?> GetItemByNameAndGameAsync(string name, int game)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT TOP 1 Id, ExternalId, Name, Image, Price, Game, MarketUrl, Status, CreatedAt, UpdatedAt
            FROM SteamItem
            WHERE Name = @Name AND Game = @Game
            ORDER BY Id DESC";
        return await db.QueryFirstOrDefaultAsync<SteamItem>(sql, new { Name = name, Game = game });
    }

    public async Task<int> CreateItems(SteamItem item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO SteamItem (ExternalId, Name, Image, Price, Game, MarketUrl, Status, CreatedAt)
            VALUES (@ExternalId, @Name, @Image, @Price, @Game, @MarketUrl, @Status, @CreatedAt);
            SELECT SCOPE_IDENTITY();";
        return await db.QuerySingleAsync<int>(sql, item);
    }

    public async Task<bool> UpdateItems(SteamItem item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE SteamItem SET ExternalId = @ExternalId, Name = @Name, Image = @Image, Price = @Price,
            Game = @Game, MarketUrl = @MarketUrl, Status = @Status, UpdatedAt = @UpdatedAt WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, item);
        return rows > 0;
    }

    public async Task<bool> DeleteItems(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM SteamItem WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsItems(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT COUNT(1) FROM SteamItem WHERE Id = @Id";
        return await db.QuerySingleAsync<int>(sql, new { Id = id }) > 0;
    }

    public async Task<bool> ExistsByExternalIdItems(string externalId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT COUNT(1) FROM SteamItem WHERE ExternalId = @ExternalId";
        return await db.QuerySingleAsync<int>(sql, new { ExternalId = externalId }) > 0;
    }

    #endregion

    #region steam-item-drop

    public async Task<IEnumerable<SteamItemDrop>> GetAllDrops()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT d.Id, d.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
                   s.MarketUrl AS ItemMarketUrl, s.Game AS ItemGame, d.Quantity, d.Price, d.SalePrice, d.Total, d.CreatedAt
            FROM SteamItemDrop d
            INNER JOIN SteamItem s ON s.Id = d.SteamItemId
            ORDER BY d.Quantity DESC";
        return await db.QueryAsync<SteamItemDrop>(sql);
    }

    public async Task<SteamItemDrop> GetByIdDrops(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT d.Id, d.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
                   s.MarketUrl AS ItemMarketUrl, s.Game AS ItemGame, d.Quantity, d.Price, d.SalePrice, d.Total, d.CreatedAt
            FROM SteamItemDrop d
            INNER JOIN SteamItem s ON s.Id = d.SteamItemId
            WHERE d.Id = @Id";
        return await db.QueryFirstOrDefaultAsync<SteamItemDrop>(sql, new { Id = id });
    }

    public async Task<int> CreateDrops(SteamItemDrop item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO SteamItemDrop (SteamItemId, Quantity, Price, SalePrice, Total, CreatedAt)
            VALUES (@SteamItemId, @Quantity, @Price, @SalePrice, @Total, @CreatedAt);
            SELECT SCOPE_IDENTITY();";
        return await db.QuerySingleAsync<int>(sql, item);
    }

    public async Task<bool> UpdateDrops(SteamItemDrop item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE SteamItemDrop SET SteamItemId = @SteamItemId, Quantity = @Quantity,
            Price = @Price, SalePrice = @SalePrice, Total = @Total WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, item);
        return rows > 0;
    }

    public async Task<bool> DeleteDrops(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM SteamItemDrop WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsDrops(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT COUNT(1) FROM SteamItemDrop WHERE Id = @Id";
        return await db.QuerySingleAsync<int>(sql, new { Id = id }) > 0;
    }
    #endregion

    #region  steam-item-purchase
    public async Task<IEnumerable<SteamItemPurchase>> GetAllPurchase()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT p.Id, p.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
                   s.MarketUrl AS ItemMarketUrl, s.Game AS ItemGame, p.PurchasePrice, p.SalePrice, p.Profit,
                   p.Status, p.PurchaseDate, p.SaleDate, p.CreatedAt
            FROM SteamItemPurchase p
            INNER JOIN SteamItem s ON s.Id = p.SteamItemId
            ORDER BY s.Game ASC, s.Name ASC, p.PurchaseDate DESC";
        return await db.QueryAsync<SteamItemPurchase>(sql);
    }

    public async Task<SteamItemPurchase> GetByIdPurchase(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT p.Id, p.SteamItemId, s.Name AS ItemName, s.Image AS ItemImage,
                   s.MarketUrl AS ItemMarketUrl, s.Game AS ItemGame, p.PurchasePrice, p.SalePrice, p.Profit,
                   p.Status, p.PurchaseDate, p.SaleDate, p.CreatedAt
            FROM SteamItemPurchase p
            INNER JOIN SteamItem s ON s.Id = p.SteamItemId
            WHERE p.Id = @Id";
        return await db.QueryFirstOrDefaultAsync<SteamItemPurchase>(sql, new { Id = id });
    }

    public async Task<int> CreatePurchase(SteamItemPurchase item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            INSERT INTO SteamItemPurchase (SteamItemId, PurchasePrice, SalePrice, Profit, Status, PurchaseDate, SaleDate, CreatedAt)
            VALUES (@SteamItemId, @PurchasePrice, @SalePrice, @Profit, @Status, @PurchaseDate, @SaleDate, @CreatedAt);
            SELECT SCOPE_IDENTITY();";
        return await db.QuerySingleAsync<int>(sql, item);
    }

    public async Task<bool> UpdatePurchase(SteamItemPurchase item)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            UPDATE SteamItemPurchase SET SteamItemId = @SteamItemId, PurchasePrice = @PurchasePrice,
            SalePrice = @SalePrice, Profit = @Profit, Status = @Status,
            PurchaseDate = @PurchaseDate, SaleDate = @SaleDate WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, item);
        return rows > 0;
    }

    public async Task<bool> DeletePurchase(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM SteamItemPurchase WHERE Id = @Id";
        int rows = await db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsPurchase(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT COUNT(1) FROM SteamItemPurchase WHERE Id = @Id";
        return await db.QuerySingleAsync<int>(sql, new { Id = id }) > 0;
    }

   #endregion

   #region steam-item-cache

    public async Task<IEnumerable<DotaCache>> GetAllCache()
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT c.Id,
                   c.TreasureId,
                   c.HeroId,
                   c.Name,
                   c.Photo,
                   c.Price,
                   c.Quantity,
                   c.Total,
                   c.Owner,
                   c.CreatedAt,
                   h.Id,
                   h.Name,
                   h.Image,
                   h.CreatedAt,
                   t.Id,
                   t.Name,
                   t.Image,
                   t.ImagePresentation,
                   t.Year,
                   t.Type,
                   t.CreatedAt
            FROM DotaCache c
            INNER JOIN DotaTreasure t ON t.Id = c.TreasureId
            INNER JOIN DotaHero h ON h.Id = c.HeroId
            ORDER BY t.Name, h.Name";

        return await db.QueryAsync<DotaCache, DotaHero, DotaTreasure, DotaCache>(
            sql,
            (cache, hero, treasure) =>
            {
                cache.Hero = hero;
                cache.Treasure = treasure;
                return cache;
            },
            splitOn: "Id,Id");
    }

    public async Task<IEnumerable<DotaCache>> GetByTreasureIdCache(int treasureId)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT c.Id,
                   c.TreasureId,
                   c.HeroId,
                   c.Name,
                   c.Photo,
                   c.Price,
                   c.Quantity,
                   c.Total,
                   c.Owner,
                   c.CreatedAt,
                   h.Id,
                   h.Name,
                   h.Image,
                   h.CreatedAt,
                   t.Id,
                   t.Name,
                   t.Image,
                   t.ImagePresentation,
                   t.Year,
                   t.Type,
                   t.CreatedAt
            FROM DotaCache c
            INNER JOIN DotaTreasure t ON t.Id = c.TreasureId
            INNER JOIN DotaHero h ON h.Id = c.HeroId
            WHERE c.TreasureId = @TreasureId
            ORDER BY h.Name";

        return await db.QueryAsync<DotaCache, DotaHero, DotaTreasure, DotaCache>(
            sql,
            (cache, hero, treasure) =>
            {
                cache.Hero = hero;
                cache.Treasure = treasure;
                return cache;
            },
            new { TreasureId = treasureId },
            splitOn: "Id,Id");
    }

    public async Task<DotaCache?> GetByIdCache(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
            SELECT c.Id,
                   c.TreasureId,
                   c.HeroId,
                   c.Name,
                   c.Photo,
                   c.Price,
                   c.Quantity,
                   c.Total,
                   c.Owner,
                   c.CreatedAt,
                   h.Id,
                   h.Name,
                   h.Image,
                   h.CreatedAt,
                   t.Id,
                   t.Name,
                   t.Image,
                   t.ImagePresentation,
                   t.Year,
                   t.Type,
                   t.CreatedAt
            FROM DotaCache c
            INNER JOIN DotaTreasure t ON t.Id = c.TreasureId
            INNER JOIN DotaHero h ON h.Id = c.HeroId
            WHERE c.Id = @Id";

        var items = await db.QueryAsync<DotaCache, DotaHero, DotaTreasure, DotaCache>(
            sql,
            (cache, hero, treasure) =>
            {
                cache.Hero = hero;
                cache.Treasure = treasure;
                return cache;
            },
            new { Id = id },
            splitOn: "Id,Id");

        return items.FirstOrDefault();
    }

    public async Task<int> CreateCache(DotaCache item)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaCache (TreasureId, HeroId, Name, Photo, Price, Quantity, Total, Owner, CreatedAt)
            VALUES (@TreasureId, @HeroId, @Name, @Photo, @Price, @Quantity, @Total, @Owner, @CreatedAt);
            SELECT SCOPE_IDENTITY();", item);
    }

    public async Task<bool> UpdateCache(DotaCache item)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE DotaCache SET TreasureId = @TreasureId, HeroId = @HeroId, Name = @Name,
            Photo = @Photo, Price = @Price, Quantity = @Quantity, Total = @Total, Owner = @Owner
            WHERE Id = @Id", item);
        return rows > 0;
    }

    public async Task<bool> DeleteCache(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaCache WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsCache(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaCache WHERE Id = @Id", new { Id = id }) > 0;
    }
   #endregion

   #region steam-hero
    public async Task<IEnumerable<DotaHero>> GetAllHero()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaHero>(
            "SELECT Id, Name, Image, CreatedAt FROM DotaHero ORDER BY Name ASC");
    }

    public async Task<DotaHero?> GetByIdHero(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<DotaHero>(
            "SELECT Id, Name, Image, CreatedAt FROM DotaHero WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateHero(DotaHero hero)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaHero (Name, Image, CreatedAt)
            VALUES (@Name, @Image, @CreatedAt);
            SELECT SCOPE_IDENTITY();", hero);
    }

    public async Task<bool> UpdateHero(DotaHero hero)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(
            "UPDATE DotaHero SET Name = @Name, Image = @Image WHERE Id = @Id", hero);
        return rows > 0;
    }

    public async Task<bool> DeleteHero(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaHero WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsHero(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaHero WHERE Id = @Id", new { Id = id }) > 0;
    }
   #endregion

   #region steam-treasure
     public async Task<IEnumerable<DotaTreasure>> GetAllTreasure()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaTreasure>(
            "SELECT Id, Name, Image, ImagePresentation, Year, Type, CreatedAt FROM DotaTreasure ORDER BY Year DESC, Name ASC");
    }

    public async Task<DotaTreasure?> GetByIdTreasure(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<DotaTreasure>(
            "SELECT Id, Name, Image, ImagePresentation, Year, Type, CreatedAt FROM DotaTreasure WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> CreateTreasure(DotaTreasure treasure)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaTreasure (Name, Image, ImagePresentation, Year, Type, CreatedAt)
            VALUES (@Name, @Image, @ImagePresentation, @Year, @Type, @CreatedAt);
            SELECT SCOPE_IDENTITY();", treasure);
    }
 
    public async Task<bool> UpdateTreasure(DotaTreasure treasure)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE DotaTreasure SET Name = @Name, Image = @Image, ImagePresentation = @ImagePresentation,
            Year = @Year, Type = @Type WHERE Id = @Id", treasure);
        return rows > 0;
    }

    public async Task<bool> DeleteTreasure(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaTreasure WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> ExistsTreasure(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaTreasure WHERE Id = @Id", new { Id = id }) > 0;
    }
   #endregion

}
