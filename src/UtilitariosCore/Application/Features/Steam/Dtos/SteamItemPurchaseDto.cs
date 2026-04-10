using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Application.Features.SteamItems.Dtos;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;

public class SteamItemPurchaseDto
{
    public int Id { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? Profit { get; set; }
    public PurchaseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public SteamItemRefDto Item { get; set; } = new();

    
}

public class CreateSteamItemPurchaseDto
{
    public int SteamItemId { get; set; }
    public decimal PurchasePrice { get; set; }
}

public class UpdateSteamItemPurchaseDto
{
    public int SteamItemId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
}
