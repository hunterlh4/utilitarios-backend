namespace UtilitariosCore.Application.Features.AnimeGaleries.Dtos;

public class AnimeGaleryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MediaDto> Media { get; set; } = new();
    public List<AnimeGaleryLinkDto> Links { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
