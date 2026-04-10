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
