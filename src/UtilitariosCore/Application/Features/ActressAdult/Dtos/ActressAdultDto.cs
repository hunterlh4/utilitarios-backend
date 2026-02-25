namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
}

