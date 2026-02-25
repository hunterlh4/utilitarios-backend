namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class UpdateJavDto
{
    public string Code { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = new();
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();
}
