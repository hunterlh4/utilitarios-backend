namespace UtilitariosCore.Domain.Models;

public class SteamItemDrop
{
    public int Id { get; set; }
    public int SteamItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}
