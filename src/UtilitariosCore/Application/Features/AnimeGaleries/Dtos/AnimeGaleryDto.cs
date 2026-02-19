namespace UtilitariosCore.Application.Features.AnimeGaleries.Dtos;

public class AnimeGaleryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AnimeGaleryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MediaDto> Media { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class MediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public int OrderIndex { get; set; }
}

public class CreateAnimeGaleryDto
{
    public int Id { get; set; }
}

public class UpdateAnimeGaleryDto
{
    public string Name { get; set; } = string.Empty;
    public int? MediaId { get; set; }
}
