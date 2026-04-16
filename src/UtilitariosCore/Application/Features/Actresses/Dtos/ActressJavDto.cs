using UtilitariosCore.Application.Features.Actresses.Dtos;

namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; } = [];
    public IEnumerable<LinkDto> Links { get; set; } = [];
    public int JavCount { get; set; }
}
