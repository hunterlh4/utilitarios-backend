using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ActressJavDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ActressLinkDto> Links { get; set; } = [];
    public List<JavSummaryDto> Javs { get; set; } = [];
}

public class ActressLinkDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}

public class JavSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
}

public class CreateActressJavDto
{
    public int Id { get; set; }
}
