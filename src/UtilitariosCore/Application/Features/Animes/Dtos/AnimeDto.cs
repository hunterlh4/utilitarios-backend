using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Animes.Dtos;

public class AnimeDto
{
    public int Id { get; set; }
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAnimeDto
{
    public int Id { get; set; }
}

public class UpdateAnimeDto
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }
}
