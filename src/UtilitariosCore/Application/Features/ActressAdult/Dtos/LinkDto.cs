namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class LinkDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}
