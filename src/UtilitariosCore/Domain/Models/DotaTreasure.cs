using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class DotaTreasure
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? ImagePresentation { get; set; }
    public int Year { get; set; }
    public TreasureType? Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
