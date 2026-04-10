using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.DotaTreasures.Dtos;

public class DotaTreasureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? ImagePresentation { get; set; }
    public int Year { get; set; }
    public TreasureType? Type { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDotaTreasureDto
{
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? ImagePresentation { get; set; }
    public int Year { get; set; }
    public TreasureType? Type { get; set; }
}

public class UpdateDotaTreasureDto
{
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? ImagePresentation { get; set; }
    public int? Year { get; set; }
    public TreasureType? Type { get; set; }
}
