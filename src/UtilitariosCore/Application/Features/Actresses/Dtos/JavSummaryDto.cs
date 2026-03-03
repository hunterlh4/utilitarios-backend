using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class JavSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<JavActressSummaryDto> Actresses { get; set; } = [];
    public List<string> Links { get; set; } = [];
}

public class JavActressSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
