namespace UtilitariosCore.Domain.Models;

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
}
