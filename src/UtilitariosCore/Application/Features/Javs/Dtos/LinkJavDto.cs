namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class LinkJavDto
{
    public int Id { get; set; }
    public int JavId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}