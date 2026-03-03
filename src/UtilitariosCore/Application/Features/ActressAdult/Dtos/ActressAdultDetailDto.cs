namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<LinkDto> Links { get; set; } = [];
}
