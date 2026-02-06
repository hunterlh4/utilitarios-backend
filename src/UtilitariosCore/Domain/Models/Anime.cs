using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Anime
{
    public int Id { get; set; }
    public required string ApiId { get; set; }
    public required string Title { get; set; }
    public required string Image { get; set; }
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
