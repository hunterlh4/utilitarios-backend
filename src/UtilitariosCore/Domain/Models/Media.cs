using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Media
{
    public int Id { get; set; }
    public MediaType Type { get; set; }
    public int RefId { get; set; }
    public required string Url { get; set; }
    public string? Thumbnail { get; set; }
    public string? DeleteUrl { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
