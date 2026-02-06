using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Javs.Dtos;

public class JavDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public ActressDto? Actress { get; set; }
    public string Image { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
    public List<LinkDto> Links { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class ActressDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}

public class LinkDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class CreateJavDto
{
    public int Id { get; set; }
}

public class UpdateJavDto
{
    public string Code { get; set; } = string.Empty;
    public string ActressName { get; set; } = string.Empty;
    public string? ActressUrl { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();
}
