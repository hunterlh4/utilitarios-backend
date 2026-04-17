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
    public string? Tags { get; set; }
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
