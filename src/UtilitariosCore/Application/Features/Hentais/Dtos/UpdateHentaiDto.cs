using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Hentais.Dtos;

public class UpdateHentaiDto
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }
}
