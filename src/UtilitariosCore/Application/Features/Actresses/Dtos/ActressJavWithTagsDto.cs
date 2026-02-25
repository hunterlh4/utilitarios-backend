namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavWithTagsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
}
