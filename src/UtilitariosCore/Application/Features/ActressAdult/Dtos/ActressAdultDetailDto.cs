namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public List<LinkDto> Links { get; set; } = [];
    public List<VideoAdultDto> Videos { get; set; } = [];
}
