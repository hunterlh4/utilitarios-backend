namespace UtilitariosCore.Application.Features.DotaCaches.Dtos;

public class DotaCacheDto
{
    public int Id { get; set; }
    public int TreasureId { get; set; }
    public int HeroId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Total { get; set; }
    public string? Owner { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDotaCacheDto
{
    public int TreasureId { get; set; }
    public int HeroId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Total { get; set; }
    public string? Owner { get; set; }
}

public class UpdateDotaCacheDto
{
    public int? TreasureId { get; set; }
    public int? HeroId { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Total { get; set; }
    public string? Owner { get; set; }
}
