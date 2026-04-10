using UtilitariosCore.Application.Features.SteamItems.Dtos;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Dtos;

public class SteamItemDropDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public SteamItemRefDto Item { get; set; } = new();
}

public class CreateItemDropDto
{
    public int SteamItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
}

public class UpdateItemDropDto
{
    public int SteamItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
}
