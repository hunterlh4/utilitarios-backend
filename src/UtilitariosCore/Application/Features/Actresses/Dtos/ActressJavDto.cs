using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; } = [];
    public IEnumerable<Link> Links { get; set; } = [];
    public int JavCount { get; set; }
}
