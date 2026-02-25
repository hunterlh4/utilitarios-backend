using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class JavDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public ActressDto? Actress { get; set; }
    public string Image { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
    public List<LinkDto> Links { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
