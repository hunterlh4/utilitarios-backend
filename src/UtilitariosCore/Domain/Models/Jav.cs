using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Jav
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public int? ActressId { get; set; }
    public required string Image { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
