using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Link
{
    public int Id { get; set; }
    public LinkType Type { get; set; }
    public int? RefId { get; set; }
    public string Name { get; set; } = string.Empty;
    public required string Url { get; set; }
    public int? OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
