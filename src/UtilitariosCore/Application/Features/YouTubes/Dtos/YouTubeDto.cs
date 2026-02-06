using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.YouTubes.Dtos;

public class YouTubeDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
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

public class CreateYouTubeDto
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
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
}

public class UpdateYouTubeDto
{
    public string? Url { get; set; }
    public string? Title { get; set; }
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
    public YouTubeCategory? Category { get; set; }
}
