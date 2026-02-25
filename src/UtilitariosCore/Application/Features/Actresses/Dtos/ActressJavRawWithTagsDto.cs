namespace UtilitariosCore.Application.Features.Actresses.Dtos;

internal class ActressJavRawWithTagsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? TagsRaw { get; set; }
}
