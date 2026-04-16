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
