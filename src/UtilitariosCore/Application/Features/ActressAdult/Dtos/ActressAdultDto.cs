using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.ActressAdults.Dtos;

public class ActressAdultDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ActressAdultDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<LinkDto> Links { get; set; } = [];
    public List<VideoAdultDto> Videos { get; set; } = [];
}

public class LinkDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}

public class VideoAdultDto
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ContentStatus Status { get; set; }
    public List<ActressSimpleDto> Actresses { get; set; } = [];
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

public class CreateVideoAdultDto
{
    public int Id { get; set; }
}

// Tipos internos para mapeo en repositorio
public class VideoAdultGrouped
{
    public int VideoId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime VideoCreatedAt { get; set; }
    public List<ActressInfo> Actresses { get; set; } = [];
}

public class ActressInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

