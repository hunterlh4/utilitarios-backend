namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class UpdateJavDto
{
    public string Code { get; set; } = string.Empty;
    public string ActressName { get; set; } = string.Empty;
    public string? ActressUrl { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();
}
