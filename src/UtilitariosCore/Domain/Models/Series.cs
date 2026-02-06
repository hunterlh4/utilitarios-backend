using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Series
{
    public int Id { get; set; }
    public required string ImdbId { get; set; }
    public required string Title { get; set; }
    public required string Image { get; set; }
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
