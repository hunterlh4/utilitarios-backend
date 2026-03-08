namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultBasicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MediaDto> Images { get; set; } = new();
}

public class MediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class ActressAdultDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
}

