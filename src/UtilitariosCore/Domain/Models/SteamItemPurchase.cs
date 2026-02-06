using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class SteamItemPurchase
{
    public int Id { get; set; }
    public int SteamItemId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? Profit { get; set; }
    public PurchaseStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? SaleDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
