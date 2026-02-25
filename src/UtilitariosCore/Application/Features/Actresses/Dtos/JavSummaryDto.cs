using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class JavSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ContentStatus Status { get; set; }
}
