using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace UtilitariosCore.Shared.Utils;

public static class ExcelHelper
{
    /// <summary>
    /// Crea un archivo Excel con la lista de PropertyItems
    /// </summary>
    /// <param name="items">Lista de items con SKU, Nombre y Ambiente</param>
    /// <returns>Stream del archivo Excel</returns>
    public static MemoryStream CreatePropertyItemsExcel(List<PropertyItemExcelRow> items)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("PropertyItems");

        // Encabezados
        worksheet.Cells[1, 1].Value = "SKU";
        worksheet.Cells[1, 2].Value = "Nombre";
        worksheet.Cells[1, 3].Value = "Ambiente";

        // Estilo de encabezados
        var headerRange = worksheet.Cells[1, 1, 1, 3];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        // Datos
        int row = 2;
        foreach (var item in items)
        {
            worksheet.Cells[row, 1].Value = item.Sku;
            worksheet.Cells[row, 2].Value = item.Name;
            worksheet.Cells[row, 3].Value = item.RoomName;
            row++;
        }

        // Ajustar ancho de columnas
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Guardar en MemoryStream
        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Lee filas de PropertyItems desde un archivo Excel (primera hoja, desde la fila 2).
    /// </summary>
    public static List<PropertyItemExcelRow> ReadPropertyItemsExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<PropertyItemExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var sku = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            var name = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var roomName = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(sku) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(roomName))
                continue;

            result.Add(new PropertyItemExcelRow
            {
                Sku = sku,
                Name = name,
                RoomName = roomName,
            });
        }

        return result;
    }
}

public class PropertyItemExcelRow
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
}
