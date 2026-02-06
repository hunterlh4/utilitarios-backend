using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class YouTube
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public required string Title { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorUrl { get; set; }
    public string? Type { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public string? Version { get; set; }
    public string? ProviderName { get; set; }
    public string? ProviderUrl { get; set; }
    public int? ThumbnailHeight { get; set; }
    public int? ThumbnailWidth { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Html { get; set; }
    public YouTubeCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
}
