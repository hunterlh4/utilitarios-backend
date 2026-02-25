namespace UtilitariosCore.Application.Features.Actresses.Dtos;

internal class ActressJavRawDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Image { get; set; }
    public string? TagsRaw { get; set; }
}
