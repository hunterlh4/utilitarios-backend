using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class DotaMedia
{
    public int Id { get; set; }
    public DotaMediaType Type { get; set; }
    public int RefId { get; set; }
    public required string Url { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
