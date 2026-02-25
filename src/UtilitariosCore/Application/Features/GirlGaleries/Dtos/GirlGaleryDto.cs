namespace UtilitariosCore.Application.Features.GirlGaleries.Dtos;

public class GirlGaleryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
