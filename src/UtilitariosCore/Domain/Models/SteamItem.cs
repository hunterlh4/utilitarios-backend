using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class SteamItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? Price { get; set; }
    public GameType Game { get; set; }
    public required string MarketUrl { get; set; }
    public SteamItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
