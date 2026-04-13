using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;

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

            int game;
            if (int.TryParse(gameText, out var numericGame))
            {
                game = numericGame;
            }
            else if (Enum.TryParse<GameType>(gameText, true, out var enumGame))
            {
                game = (int)enumGame;
            }
            else
            {
                game = 0;
            }

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

    public static MemoryStream CreateSteamItemDropsExcel(List<SteamItemDrop> drops)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("SteamDrops");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "SteamItemId";
        worksheet.Cells[1, 3].Value = "Quantity";
        worksheet.Cells[1, 4].Value = "Price";
        worksheet.Cells[1, 5].Value = "SalePrice";
        worksheet.Cells[1, 6].Value = "Total";
        worksheet.Cells[1, 7].Value = "CreatedAt";

        var headerRange = worksheet.Cells[1, 1, 1, 7];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var drop in drops)
        {
            worksheet.Cells[row, 1].Value = drop.Id;
            worksheet.Cells[row, 2].Value = drop.SteamItemId;
            worksheet.Cells[row, 3].Value = drop.Quantity;
            worksheet.Cells[row, 4].Value = drop.Price;
            worksheet.Cells[row, 5].Value = drop.SalePrice;
            worksheet.Cells[row, 6].Value = drop.Total;
            worksheet.Cells[row, 7].Value = drop.CreatedAt;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<SteamItemDrop> ReadSteamItemDropsExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<SteamItemDrop>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var steamItemIdText = worksheet.Cells[row, 2].Text?.Trim() ?? "0";
            var quantityText = worksheet.Cells[row, 3].Text?.Trim() ?? "0";
            var priceText = worksheet.Cells[row, 4].Text?.Trim() ?? "0";
            var salePriceText = worksheet.Cells[row, 5].Text?.Trim() ?? "0";
            var createdAtText = worksheet.Cells[row, 7].Text?.Trim();

            if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(steamItemIdText) &&
                string.IsNullOrWhiteSpace(quantityText) && string.IsNullOrWhiteSpace(priceText) &&
                string.IsNullOrWhiteSpace(salePriceText) && string.IsNullOrWhiteSpace(createdAtText))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(steamItemIdText, out var steamItemId);
            int.TryParse(quantityText, out var quantity);

            if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) &&
                !decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.CurrentCulture, out price))
            {
                price = -1;
            }

            if (!decimal.TryParse(salePriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var salePrice) &&
                !decimal.TryParse(salePriceText, NumberStyles.Any, CultureInfo.CurrentCulture, out salePrice))
            {
                salePrice = -1;
            }

            DateTime.TryParse(createdAtText, CultureInfo.CurrentCulture, DateTimeStyles.None, out var createdAt);
            if (createdAt == default)
            {
                DateTime.TryParse(createdAtText, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdAt);
            }

            result.Add(new SteamItemDrop
            {
                Id = id,
                SteamItemId = steamItemId,
                Quantity = quantity,
                Price = price,
                SalePrice = salePrice,
                Total = quantity * salePrice,
                CreatedAt = createdAt == default ? DateTime.Now : createdAt,
            });
        }

        return result;
    }

    public static MemoryStream CreateSteamItemPurchasesExcel(List<SteamItemPurchase> purchases)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("SteamPurchases");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "SteamItemId";
        worksheet.Cells[1, 3].Value = "PurchasePrice";
        worksheet.Cells[1, 4].Value = "SalePrice";
        worksheet.Cells[1, 5].Value = "Profit";
        worksheet.Cells[1, 6].Value = "Status";
        worksheet.Cells[1, 7].Value = "PurchaseDate";
        worksheet.Cells[1, 8].Value = "SaleDate";
        worksheet.Cells[1, 9].Value = "CreatedAt";

        var headerRange = worksheet.Cells[1, 1, 1, 9];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var purchase in purchases)
        {
            worksheet.Cells[row, 1].Value = purchase.Id;
            worksheet.Cells[row, 2].Value = purchase.SteamItemId;
            worksheet.Cells[row, 3].Value = purchase.PurchasePrice;
            worksheet.Cells[row, 4].Value = purchase.SalePrice;
            worksheet.Cells[row, 5].Value = purchase.Profit;
            worksheet.Cells[row, 6].Value = purchase.Status.ToString();
            worksheet.Cells[row, 7].Value = purchase.PurchaseDate;
            worksheet.Cells[row, 8].Value = purchase.SaleDate;
            worksheet.Cells[row, 9].Value = purchase.CreatedAt;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<SteamItemPurchase> ReadSteamItemPurchasesExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<SteamItemPurchase>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var steamItemIdText = worksheet.Cells[row, 2].Text?.Trim() ?? "0";
            var purchasePriceText = worksheet.Cells[row, 3].Text?.Trim() ?? "0";
            var salePriceText = worksheet.Cells[row, 4].Text?.Trim() ?? "0";
            var statusText = worksheet.Cells[row, 6].Text?.Trim();
            var purchaseDateText = worksheet.Cells[row, 7].Text?.Trim();
            var saleDateText = worksheet.Cells[row, 8].Text?.Trim();
            var createdAtText = worksheet.Cells[row, 9].Text?.Trim();

            if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(steamItemIdText) &&
                string.IsNullOrWhiteSpace(purchasePriceText) && string.IsNullOrWhiteSpace(salePriceText) &&
                string.IsNullOrWhiteSpace(statusText) && string.IsNullOrWhiteSpace(purchaseDateText) &&
                string.IsNullOrWhiteSpace(saleDateText) && string.IsNullOrWhiteSpace(createdAtText))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(steamItemIdText, out var steamItemId);

            if (!decimal.TryParse(purchasePriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var purchasePrice) &&
                !decimal.TryParse(purchasePriceText, NumberStyles.Any, CultureInfo.CurrentCulture, out purchasePrice))
            {
                purchasePrice = -1;
            }

            if (!decimal.TryParse(salePriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var salePrice) &&
                !decimal.TryParse(salePriceText, NumberStyles.Any, CultureInfo.CurrentCulture, out salePrice))
            {
                salePrice = -1;
            }

            var status = PurchaseStatus.Comprado;
            if (!string.IsNullOrWhiteSpace(statusText))
            {
                if (int.TryParse(statusText, out var numericStatus) && Enum.IsDefined(typeof(PurchaseStatus), numericStatus))
                {
                    status = (PurchaseStatus)numericStatus;
                }
                else if (Enum.TryParse<PurchaseStatus>(statusText, true, out var enumStatus))
                {
                    status = enumStatus;
                }
            }

            DateTime.TryParse(purchaseDateText, CultureInfo.CurrentCulture, DateTimeStyles.None, out var purchaseDate);
            if (purchaseDate == default)
            {
                DateTime.TryParse(purchaseDateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate);
            }

            DateTime.TryParse(saleDateText, CultureInfo.CurrentCulture, DateTimeStyles.None, out var saleDate);
            if (saleDate == default)
            {
                DateTime.TryParse(saleDateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out saleDate);
            }

            DateTime.TryParse(createdAtText, CultureInfo.CurrentCulture, DateTimeStyles.None, out var createdAt);
            if (createdAt == default)
            {
                DateTime.TryParse(createdAtText, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdAt);
            }

            result.Add(new SteamItemPurchase
            {
                Id = id,
                SteamItemId = steamItemId,
                PurchasePrice = purchasePrice,
                SalePrice = salePrice,
                Profit = salePrice > 0 ? salePrice - purchasePrice : null,
                Status = salePrice > 0 ? PurchaseStatus.Vendido : status,
                PurchaseDate = purchaseDate == default ? DateTime.Now : purchaseDate,
                SaleDate = salePrice > 0 ? (saleDate == default ? DateTime.Now : saleDate) : null,
                CreatedAt = createdAt == default ? DateTime.Now : createdAt,
            });
        }

        return result;
    }

    public static MemoryStream CreateActressJavExcel(List<ActressJav> actresses)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("ActressJav");

        worksheet.Cells[1, 1].Value = "Name";
        worksheet.Cells[1, 2].Value = "Image";

        var headerRange = worksheet.Cells[1, 1, 1, 2];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var actress in actresses)
        {
            worksheet.Cells[row, 1].Value = actress.Name;
            worksheet.Cells[row, 2].Value = actress.Image;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<ActressJav> ReadActressJavExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<ActressJav>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var name = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 2].Text?.Trim();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image))
                continue;

            result.Add(new ActressJav
            {
                Name = name,
                Image = image,
            });
        }

        return result;
    }

    public static MemoryStream CreateActressAdultExcel(List<ActressAdult> actresses)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("ActressAdult");

        worksheet.Cells[1, 1].Value = "Name";
        worksheet.Cells[1, 2].Value = "Image";

        var headerRange = worksheet.Cells[1, 1, 1, 2];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var actress in actresses)
        {
            worksheet.Cells[row, 1].Value = actress.Name;
            worksheet.Cells[row, 2].Value = actress.Image;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<ActressAdult> ReadActressAdultExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<ActressAdult>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var name = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 2].Text?.Trim();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image))
                continue;

            result.Add(new ActressAdult
            {
                Name = name,
                Image = image,
            });
        }

        return result;
    }

    public static MemoryStream CreateAnimeGaleryExcel(List<AnimeGalery> galeries, List<GaleryMediaExcelRow> mediaRows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();

        var galerySheet = package.Workbook.Worksheets.Add("AnimeGaleries");
        galerySheet.Cells[1, 1].Value = "Id";
        galerySheet.Cells[1, 2].Value = "Name";
        galerySheet.Cells[1, 3].Value = "Image";

        var galeryHeader = galerySheet.Cells[1, 1, 1, 3];
        galeryHeader.Style.Font.Bold = true;
        galeryHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        galeryHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        int row = 2;
        foreach (var galery in galeries)
        {
            galerySheet.Cells[row, 1].Value = galery.Id;
            galerySheet.Cells[row, 2].Value = galery.Name;
            galerySheet.Cells[row, 3].Value = galery.Image;
            row++;
        }

        var mediaSheet = package.Workbook.Worksheets.Add("AnimeGaleryMedia");
        mediaSheet.Cells[1, 1].Value = "AnimeGaleryId";
        mediaSheet.Cells[1, 2].Value = "Url";
        mediaSheet.Cells[1, 3].Value = "OrderIndex";

        var mediaHeader = mediaSheet.Cells[1, 1, 1, 3];
        mediaHeader.Style.Font.Bold = true;
        mediaHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        mediaHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var media in mediaRows)
        {
            mediaSheet.Cells[row, 1].Value = media.GaleryId;
            mediaSheet.Cells[row, 2].Value = media.Url;
            mediaSheet.Cells[row, 3].Value = media.OrderIndex;
            row++;
        }

        if (galerySheet.Dimension is not null)
            galerySheet.Cells[galerySheet.Dimension.Address].AutoFitColumns();
        if (mediaSheet.Dimension is not null)
            mediaSheet.Cells[mediaSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static GaleryExcelData ReadAnimeGaleryExcel(Stream excelStream)
    {
        return ReadGaleryExcel(excelStream, "AnimeGaleries", "AnimeGaleryMedia", "AnimeGaleryId");
    }

    public static MemoryStream CreateGirlGaleryExcel(List<GirlGalery> galeries, List<GaleryMediaExcelRow> mediaRows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();

        var galerySheet = package.Workbook.Worksheets.Add("GirlGaleries");
        galerySheet.Cells[1, 1].Value = "Id";
        galerySheet.Cells[1, 2].Value = "Name";
        galerySheet.Cells[1, 3].Value = "Image";

        var galeryHeader = galerySheet.Cells[1, 1, 1, 3];
        galeryHeader.Style.Font.Bold = true;
        galeryHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        galeryHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        int row = 2;
        foreach (var galery in galeries)
        {
            galerySheet.Cells[row, 1].Value = galery.Id;
            galerySheet.Cells[row, 2].Value = galery.Name;
            galerySheet.Cells[row, 3].Value = galery.Image;
            row++;
        }

        var mediaSheet = package.Workbook.Worksheets.Add("GirlGaleryMedia");
        mediaSheet.Cells[1, 1].Value = "GirlGaleryId";
        mediaSheet.Cells[1, 2].Value = "Url";
        mediaSheet.Cells[1, 3].Value = "OrderIndex";

        var mediaHeader = mediaSheet.Cells[1, 1, 1, 3];
        mediaHeader.Style.Font.Bold = true;
        mediaHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        mediaHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var media in mediaRows)
        {
            mediaSheet.Cells[row, 1].Value = media.GaleryId;
            mediaSheet.Cells[row, 2].Value = media.Url;
            mediaSheet.Cells[row, 3].Value = media.OrderIndex;
            row++;
        }

        if (galerySheet.Dimension is not null)
            galerySheet.Cells[galerySheet.Dimension.Address].AutoFitColumns();
        if (mediaSheet.Dimension is not null)
            mediaSheet.Cells[mediaSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static GaleryExcelData ReadGirlGaleryExcel(Stream excelStream)
    {
        return ReadGaleryExcel(excelStream, "GirlGaleries", "GirlGaleryMedia", "GirlGaleryId");
    }

    private static GaleryExcelData ReadGaleryExcel(Stream excelStream, string galerySheetName, string mediaSheetName, string mediaGaleryIdHeader)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var galerySheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == galerySheetName)
                         ?? package.Workbook.Worksheets.FirstOrDefault();
        var mediaSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == mediaSheetName);

        var result = new GaleryExcelData();

        if (galerySheet?.Dimension is not null)
        {
            for (int row = 2; row <= galerySheet.Dimension.End.Row; row++)
            {
                var idText = galerySheet.Cells[row, 1].Text?.Trim() ?? "0";
                var name = galerySheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var image = galerySheet.Cells[row, 3].Text?.Trim();

                if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image))
                    continue;

                int.TryParse(idText, out var id);

                result.Galeries.Add(new GaleryExcelRow
                {
                    Id = id,
                    Name = name,
                    Image = image,
                });
            }
        }

        if (mediaSheet?.Dimension is not null)
        {
            var galeryIdColumn = 1;
            for (int col = 1; col <= mediaSheet.Dimension.End.Column; col++)
            {
                if (string.Equals(mediaSheet.Cells[1, col].Text?.Trim(), mediaGaleryIdHeader, StringComparison.OrdinalIgnoreCase))
                {
                    galeryIdColumn = col;
                    break;
                }
            }

            for (int row = 2; row <= mediaSheet.Dimension.End.Row; row++)
            {
                var galeryIdText = mediaSheet.Cells[row, galeryIdColumn].Text?.Trim() ?? "0";
                var url = mediaSheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var orderText = mediaSheet.Cells[row, 3].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(galeryIdText) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(galeryIdText, out var galeryId);
                int.TryParse(orderText, out var orderIndex);

                result.Media.Add(new GaleryMediaExcelRow
                {
                    GaleryId = galeryId,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        return result;
    }
}