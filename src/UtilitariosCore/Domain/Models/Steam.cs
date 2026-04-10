using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class SteamItem
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public decimal Price { get; set; }
    public GameType Game { get; set; }
    public required string MarketUrl { get; set; }
    public SteamItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}


public class SteamItemDrop
{
    public int Id { get; set; }
    public int SteamItemId { get; set; }
    public string? ItemName { get; set; }
    public string? ItemImage { get; set; }
    public string? ItemMarketUrl { get; set; }
    public GameType? ItemGame { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public SteamItem? SteamItem { get; set; }
}


public class SteamItemPurchase
{
    public int Id { get; set; }
    public int SteamItemId { get; set; }
    public string? ItemName { get; set; }
    public string? ItemImage { get; set; }
    public string? ItemMarketUrl { get; set; }
    public GameType? ItemGame { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? Profit { get; set; }
    public PurchaseStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? SaleDate { get; set; }
    public DateTime CreatedAt { get; set; }

     public SteamItem? SteamItem { get; set; }
}


public class DotaCache
{
    public int Id { get; set; }
    public int TreasureId { get; set; }
    public int HeroId { get; set; }
    public required string Name { get; set; }
    public required string Photo { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Total { get; set; }
    public string? Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DotaHero? Hero { get; set; }
    public DotaTreasure? Treasure { get; set; }
}

public class DotaHero
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}


public class DotaTreasure
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? ImagePresentation { get; set; }
    public int Year { get; set; }
    public TreasureType? Type { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DotaMedia
{
    public int Id { get; set; }
    public DotaMediaType Type { get; set; }
    public int RefId { get; set; }
    public required string Url { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
