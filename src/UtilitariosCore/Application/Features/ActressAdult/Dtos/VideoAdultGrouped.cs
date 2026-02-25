using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class VideoAdultGrouped
{
    public int VideoId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime VideoCreatedAt { get; set; }
    public List<ActressInfo> Actresses { get; set; } = [];
}
