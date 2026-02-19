namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ActressAdultDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<VideoAdultDto> Videos { get; set; } = new();
}

public class VideoAdultDto
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public char Status { get; set; }
    public List<ActressSimpleDto> Actresses { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class ActressSimpleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateActressAdultDto
{
    public int Id { get; set; }
}

public class UpdateActressAdultDto
{
    public string Name { get; set; } = string.Empty;
}

public class CreateVideoAdultDto
{
    public int Id { get; set; }
}
