
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Models;

public class ActressAdultDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<Link> Links { get; set; } = [];
    public List<VideoAdultDto> Videos { get; set; } = [];
}
