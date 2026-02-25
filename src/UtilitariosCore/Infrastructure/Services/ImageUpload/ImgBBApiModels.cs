namespace UtilitariosCore.Infrastructure.Services.ImageUpload;

internal class ImgBBApiResponse
{
    public bool Success { get; set; }
    public ImgBBApiData? Data { get; set; }
}

internal class ImgBBApiData
{
    public string Url { get; set; } = string.Empty;
    public string DisplayUrl { get; set; } = string.Empty;
    public string DeleteUrl { get; set; } = string.Empty;
    public ImgBBApiImage? Thumb { get; set; }
    public ImgBBApiImage? Medium { get; set; }
}

internal class ImgBBApiImage
{
    public string Url { get; set; } = string.Empty;
}
