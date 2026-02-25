namespace UtilitariosCore.Application.Features.GirlGaleries.Dtos;

public class MediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public int OrderIndex { get; set; }
}
