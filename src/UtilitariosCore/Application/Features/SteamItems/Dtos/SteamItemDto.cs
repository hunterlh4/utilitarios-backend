using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.SteamItems.Dtos;

public class SteamItemDto
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public GameType Game { get; set; }
    public string MarketUrl { get; set; } = string.Empty;
    public SteamItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSteamItemDto
{
    public string? ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public GameType Game { get; set; }
    public string MarketUrl { get; set; } = string.Empty;
    public SteamItemStatus Status { get; set; }
}

public class UpdateSteamItemDto
{
    public string? ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public GameType Game { get; set; }
    public string MarketUrl { get; set; } = string.Empty;
    public SteamItemStatus Status { get; set; }
}
