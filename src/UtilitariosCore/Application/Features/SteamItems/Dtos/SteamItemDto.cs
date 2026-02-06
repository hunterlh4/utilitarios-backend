using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.SteamItems.Dtos;

public class SteamItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Price { get; set; }
    public GameType Game { get; set; }
    public string MarketUrl { get; set; } = string.Empty;
    public SteamItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSteamItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Price { get; set; }
    public GameType Game { get; set; }
    public string MarketUrl { get; set; } = string.Empty;
    public SteamItemStatus Status { get; set; }
}

public class UpdateSteamItemDto
{
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? Price { get; set; }
    public GameType? Game { get; set; }
    public string? MarketUrl { get; set; }
    public SteamItemStatus? Status { get; set; }
}
