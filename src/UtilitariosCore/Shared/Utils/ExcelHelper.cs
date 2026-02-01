using ClosedXML.Excel;

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
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("PropertyItems");

        // Encabezados
        worksheet.Cell(1, 1).Value = "SKU";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Ambiente";

        // Estilo de encabezados
        var headerRange = worksheet.Range(1, 1, 1, 3);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Datos
        int row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.Sku;
            worksheet.Cell(row, 2).Value = item.Name;
            worksheet.Cell(row, 3).Value = item.RoomName;
            row++;
        }

        // Ajustar ancho de columnas
        worksheet.Columns().AdjustToContents();

        // Guardar en MemoryStream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }
}

public class PropertyItemExcelRow
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
}
