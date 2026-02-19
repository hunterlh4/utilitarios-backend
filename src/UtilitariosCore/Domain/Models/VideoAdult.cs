namespace UtilitariosCore.Domain.Models;

public class VideoAdult
{
    public int Id { get; set; }
    public required string Source { get; set; }
    public required string ExternalId { get; set; }
    public required string VideoUrl { get; set; }
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? EmbedHtml { get; set; }
    public char Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
