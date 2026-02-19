namespace UtilitariosCore.Domain.Dtos;

public class VideoAdultWithActresses
{
    public int VideoId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public char Status { get; set; }
    public DateTime VideoCreatedAt { get; set; }
    public int ActressId { get; set; }
    public string ActressName { get; set; } = string.Empty;
}
