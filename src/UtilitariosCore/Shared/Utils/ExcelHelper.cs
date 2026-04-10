using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Shared.Utils;

public static class ExcelHelper
{
 
    public static MemoryStream CreateSteamItemsExcel(List<SteamItem> items)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("SteamItems");

        worksheet.Cells[1, 1].Value = "ExternalId";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Image";
        worksheet.Cells[1, 4].Value = "Price";
        worksheet.Cells[1, 5].Value = "Game";
        worksheet.Cells[1, 6].Value = "MarketUrl";

        var headerRange = worksheet.Cells[1, 1, 1, 6];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in items)
        {
            worksheet.Cells[row, 1].Value = item.ExternalId;
            worksheet.Cells[row, 2].Value = item.Name;
            worksheet.Cells[row, 3].Value = item.Image;
            worksheet.Cells[row, 4].Value = item.Price;
            worksheet.Cells[row, 5].Value = item.Game;
            worksheet.Cells[row, 6].Value = item.MarketUrl;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<SteamItem> ReadSteamItemsExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<SteamItem>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var externalId = worksheet.Cells[row, 1].Text?.Trim();
            var name = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var priceText = worksheet.Cells[row, 4].Text?.Trim() ?? "0";
            var gameText = worksheet.Cells[row, 5].Text?.Trim() ?? "0";
            var marketUrl = worksheet.Cells[row, 6].Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(externalId) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image)
                && string.IsNullOrWhiteSpace(priceText) && string.IsNullOrWhiteSpace(gameText) && string.IsNullOrWhiteSpace(marketUrl))
                continue;

            if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) &&
                !decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.CurrentCulture, out price))
            {
                price = -1;
            }

            if (!int.TryParse(gameText, out var game))
                game = 0;

            result.Add(new SteamItem
            {
                ExternalId = externalId,
                Name = name,
                Image = image,
                Price = price,
                Game = (GameType)game,
                MarketUrl = marketUrl,
            });
        }

        return result;
    }
}