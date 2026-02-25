namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class ActressDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
}
