namespace UtilitariosCore.Application.Features.GirlGaleries.Dtos;

public class GirlGaleryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GirlGaleryDetailDto
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

public class CreateGirlGaleryDto
{
    public int Id { get; set; }
}

public class UpdateGirlGaleryDto
{
    public string Name { get; set; } = string.Empty;
    public int? MediaId { get; set; }
}
