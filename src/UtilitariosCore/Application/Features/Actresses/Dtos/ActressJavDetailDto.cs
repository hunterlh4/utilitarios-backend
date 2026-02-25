namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<ActressLinkDto> Links { get; set; } = [];
    public List<JavSummaryDto> Javs { get; set; } = [];
}
