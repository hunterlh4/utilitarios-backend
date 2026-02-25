namespace UtilitariosCore.Application.Features.Media.Dtos;

public class UploadImageDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public string DeleteUrl { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class ImgBBUploadResponse
{
    public string Url { get; set; } = string.Empty;
    public string DisplayUrl { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public string DeleteUrl { get; set; } = string.Empty;
}
