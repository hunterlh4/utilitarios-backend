namespace UtilitariosCore.Shared.Dtos;

public class ExcelFileDto
{
    public string FileName { get; set; } = string.Empty;
    public string Base64 { get; set; } = string.Empty;
}

public class ImportExcelResult
{
    public int Created { get; set; }
    public int Updated { get; set; }
    public int Skipped { get; set; }
    public int Invalid { get; set; }
}

public class GaleryExcelRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
}

public class GaleryMediaExcelRow
{
    public int GaleryId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class GaleryLinkExcelRow
{
    public int GaleryId { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class GaleryExcelData
{
    public List<GaleryExcelRow> Galeries { get; set; } = new();
    public List<GaleryMediaExcelRow> Media { get; set; } = new();
    public List<GaleryLinkExcelRow> Links { get; set; } = new();
}

public class ActressJavExcelRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? TagIds { get; set; }
}

public class ActressJavLinkExcelRow
{
    public int ActressJavId { get; set; }
    public string? ActressJavName { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class ActressJavExcelData
{
    public List<ActressJavExcelRow> Actresses { get; set; } = new();
    public List<ActressJavLinkExcelRow> Links { get; set; } = new();
    public List<JavExcelRow> Javs { get; set; } = new();
    public List<JavLinkExcelRow> JavLinks { get; set; } = new();
}

public class JavExcelRow
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? TagIds { get; set; }
    public string? ActressIds { get; set; }
}

public class JavLinkExcelRow
{
    public int JavId { get; set; }
    public string? JavCode { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class TagExcelRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Type { get; set; }
}

public class TagExcelData
{
    public List<TagExcelRow> Tags { get; set; } = new();
}

public class AnimeExcelRow
{
    public int Id { get; set; }
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public int Status { get; set; }
}

public class HentaiExcelRow
{
    public int Id { get; set; }
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public int Status { get; set; }
    public string? TagIds { get; set; }
}

public class SeriesExcelRow
{
    public int Id { get; set; }
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }
    public int Status { get; set; }
}

public class YouTubeExcelRow
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public string? AuthorUrl { get; set; }
    public string? Type { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public string? Version { get; set; }
    public string? ProviderName { get; set; }
    public string? ProviderUrl { get; set; }
    public int? ThumbnailHeight { get; set; }
    public int? ThumbnailWidth { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Html { get; set; }
    public int Category { get; set; }
}

public class ActressAdultExcelRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? TagIds { get; set; }
}

public class ActressAdultLinkExcelRow
{
    public int ActressAdultId { get; set; }
    public string? ActressAdultName { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class VideoAdultExcelRow
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int Status { get; set; }
    public string? TagIds { get; set; }
    public string? ActressIds { get; set; }
    public string? ActressNames { get; set; }
}

public class VideoAdultLinkExcelRow
{
    public int VideoAdultId { get; set; }
    public string? VideoExternalId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class ActressAdultExcelData
{
    public List<ActressAdultExcelRow> Actresses { get; set; } = new();
    public List<ActressAdultLinkExcelRow> ActressLinks { get; set; } = new();
    public List<VideoAdultExcelRow> Videos { get; set; } = new();
    public List<VideoAdultLinkExcelRow> VideoLinks { get; set; } = new();
}

public class ComicExcelRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
