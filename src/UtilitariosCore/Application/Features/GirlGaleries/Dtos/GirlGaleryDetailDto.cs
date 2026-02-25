namespace UtilitariosCore.Application.Features.GirlGaleries.Dtos;

public class GirlGaleryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MediaDto> Media { get; set; } = new();
    public List<GirlGaleryLinkDto> Links { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
