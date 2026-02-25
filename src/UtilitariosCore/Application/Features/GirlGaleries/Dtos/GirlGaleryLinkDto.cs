namespace UtilitariosCore.Application.Features.GirlGaleries.Dtos;

public class GirlGaleryLinkDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}
