using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class VideoAdultDto
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ContentStatus Status { get; set; }
    public List<ActressSimpleDto> Actresses { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
