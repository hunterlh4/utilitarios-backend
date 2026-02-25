namespace UtilitariosCore.Application.Features.AnimeGaleries.Dtos;

public class AnimeGaleryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
