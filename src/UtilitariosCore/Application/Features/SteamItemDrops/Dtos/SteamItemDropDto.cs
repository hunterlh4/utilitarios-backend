namespace UtilitariosCore.Application.Features.SteamItemDrops.Dtos;

public class SteamItemDropDto
{
    public int Id { get; set; }
    public int SteamItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSteamItemDropDto
{
    public int SteamItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Total { get; set; }
}

public class UpdateSteamItemDropDto
{
    public int? SteamItemId { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? Total { get; set; }
}
