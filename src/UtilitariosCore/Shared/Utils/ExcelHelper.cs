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

    public static MemoryStream CreateActressJavExcel(
        List<ActressJavExcelRow> actresses,
        List<ActressJavLinkExcelRow> linkRows,
        List<JavExcelRow> javRows,
        List<JavLinkExcelRow> javLinkRows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("ActressJav");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Image";
        worksheet.Cells[1, 4].Value = "TagIds";

        var headerRange = worksheet.Cells[1, 1, 1, 4];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var actress in actresses)
        {
            worksheet.Cells[row, 1].Value = actress.Id;
            worksheet.Cells[row, 2].Value = actress.Name;
            worksheet.Cells[row, 3].Value = actress.Image;
            worksheet.Cells[row, 4].Value = actress.TagIds;
            row++;
        }

        var linkSheet = package.Workbook.Worksheets.Add("ActressJavLinks");
        linkSheet.Cells[1, 1].Value = "ActressJavId";
        linkSheet.Cells[1, 2].Value = "ActressJavName";
        linkSheet.Cells[1, 3].Value = "Url";
        linkSheet.Cells[1, 4].Value = "OrderIndex";

        var linkHeader = linkSheet.Cells[1, 1, 1, 4];
        linkHeader.Style.Font.Bold = true;
        linkHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        linkHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var link in linkRows)
        {
            linkSheet.Cells[row, 1].Value = link.ActressJavId;
            linkSheet.Cells[row, 2].Value = link.ActressJavName;
            linkSheet.Cells[row, 3].Value = link.Url;
            linkSheet.Cells[row, 4].Value = link.OrderIndex;
            row++;
        }

        var javSheet = package.Workbook.Worksheets.Add("Javs");
        javSheet.Cells[1, 1].Value = "Id";
        javSheet.Cells[1, 2].Value = "Code";
        javSheet.Cells[1, 3].Value = "Image";
        javSheet.Cells[1, 4].Value = "Status";
        javSheet.Cells[1, 5].Value = "TagIds";
        javSheet.Cells[1, 6].Value = "ActressIds";

        var javHeader = javSheet.Cells[1, 1, 1, 6];
        javHeader.Style.Font.Bold = true;
        javHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        javHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        javHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        row = 2;
        foreach (var jav in javRows)
        {
            javSheet.Cells[row, 1].Value = jav.Id;
            javSheet.Cells[row, 2].Value = jav.Code;
            javSheet.Cells[row, 3].Value = jav.Image;
            javSheet.Cells[row, 4].Value = jav.Status;
            javSheet.Cells[row, 5].Value = jav.TagIds;
            javSheet.Cells[row, 6].Value = jav.ActressIds;
            row++;
        }

        var javLinksSheet = package.Workbook.Worksheets.Add("JavLinks");
        javLinksSheet.Cells[1, 1].Value = "JavId";
        javLinksSheet.Cells[1, 2].Value = "JavCode";
        javLinksSheet.Cells[1, 3].Value = "Url";
        javLinksSheet.Cells[1, 4].Value = "OrderIndex";

        var javLinksHeader = javLinksSheet.Cells[1, 1, 1, 4];
        javLinksHeader.Style.Font.Bold = true;
        javLinksHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        javLinksHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var javLink in javLinkRows)
        {
            javLinksSheet.Cells[row, 1].Value = javLink.JavId;
            javLinksSheet.Cells[row, 2].Value = javLink.JavCode;
            javLinksSheet.Cells[row, 3].Value = javLink.Url;
            javLinksSheet.Cells[row, 4].Value = javLink.OrderIndex;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        if (linkSheet.Dimension is not null)
            linkSheet.Cells[linkSheet.Dimension.Address].AutoFitColumns();
        if (javSheet.Dimension is not null)
            javSheet.Cells[javSheet.Dimension.Address].AutoFitColumns();
        if (javLinksSheet.Dimension is not null)
            javLinksSheet.Cells[javLinksSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static ActressJavExcelData ReadActressJavExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressJav")
                        ?? package.Workbook.Worksheets.FirstOrDefault();
        var linkSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressJavLinks");
        var javSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Javs");
        var javLinksSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "JavLinks");

        var result = new ActressJavExcelData();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var name = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 3].Text?.Trim();
            var tags = worksheet.Dimension.End.Column >= 4
                ? worksheet.Cells[row, 4].Text?.Trim()
                : null;

            if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image))
                continue;

            int.TryParse(idText, out var id);

            result.Actresses.Add(new ActressJavExcelRow
            {
                Id = id,
                Name = name,
                Image = image,
                TagIds = tags,
            });
        }

        if (linkSheet?.Dimension is not null)
        {
            for (int row = 2; row <= linkSheet.Dimension.End.Row; row++)
            {
                var actressIdText = linkSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var actressName = linkSheet.Cells[row, 2].Text?.Trim();
                var url = linkSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var orderText = linkSheet.Cells[row, 4].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(actressIdText) && string.IsNullOrWhiteSpace(actressName) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(actressIdText, out var actressId);
                int.TryParse(orderText, out var orderIndex);

                result.Links.Add(new ActressJavLinkExcelRow
                {
                    ActressJavId = actressId,
                    ActressJavName = actressName,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        if (javSheet?.Dimension is not null)
        {
            for (int row = 2; row <= javSheet.Dimension.End.Row; row++)
            {
                var idText = javSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var code = javSheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var image = javSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var statusText = javSheet.Cells[row, 4].Text?.Trim() ?? "0";
                var tagIds = javSheet.Dimension.End.Column >= 5 ? javSheet.Cells[row, 5].Text?.Trim() : null;
                var actressIds = javSheet.Dimension.End.Column >= 6 ? javSheet.Cells[row, 6].Text?.Trim() : null;

                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(image))
                    continue;

                int.TryParse(idText, out var id);
                int.TryParse(statusText, out var status);

                result.Javs.Add(new JavExcelRow
                {
                    Id = id,
                    Code = code,
                    Image = image,
                    Status = status,
                    TagIds = tagIds,
                    ActressIds = actressIds,
                });
            }
        }

        if (javLinksSheet?.Dimension is not null)
        {
            for (int row = 2; row <= javLinksSheet.Dimension.End.Row; row++)
            {
                var javIdText = javLinksSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var javCode = javLinksSheet.Cells[row, 2].Text?.Trim();
                var url = javLinksSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var orderText = javLinksSheet.Cells[row, 4].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(javIdText) && string.IsNullOrWhiteSpace(javCode) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(javIdText, out var javId);
                int.TryParse(orderText, out var orderIndex);

                result.JavLinks.Add(new JavLinkExcelRow
                {
                    JavId = javId,
                    JavCode = javCode,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        return result;
    }

    public static MemoryStream CreateTagExcel(List<TagExcelRow> tags)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Tags");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Type";

        var headerRange = worksheet.Cells[1, 1, 1, 3];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var tag in tags)
        {
            worksheet.Cells[row, 1].Value = tag.Id;
            worksheet.Cells[row, 2].Value = tag.Name;
            worksheet.Cells[row, 3].Value = tag.Type;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static TagExcelData ReadTagExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Tags")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new TagExcelData();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var name = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var typeText = worksheet.Dimension.End.Column >= 3 ? worksheet.Cells[row, 3].Text?.Trim() ?? "0" : "0";

            if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(typeText))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(typeText, out var type);

            result.Tags.Add(new TagExcelRow
            {
                Id = id,
                Name = name,
                Type = type,
            });
        }

        return result;
    }

    public static MemoryStream CreateActressAdultExcel(
        List<ActressAdultExcelRow> actresses,
        List<ActressAdultLinkExcelRow> actressLinks,
        List<VideoAdultExcelRow> videos,
        List<VideoAdultLinkExcelRow> videoLinks)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var actressSheet = package.Workbook.Worksheets.Add("ActressAdult");

        actressSheet.Cells[1, 1].Value = "Id";
        actressSheet.Cells[1, 2].Value = "Name";
        actressSheet.Cells[1, 3].Value = "Image";
        actressSheet.Cells[1, 4].Value = "TagIds";

        var headerRange = actressSheet.Cells[1, 1, 1, 4];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var actress in actresses)
        {
            actressSheet.Cells[row, 1].Value = actress.Id;
            actressSheet.Cells[row, 2].Value = actress.Name;
            actressSheet.Cells[row, 3].Value = actress.Image;
            actressSheet.Cells[row, 4].Value = actress.TagIds;
            row++;
        }

        var actressLinksSheet = package.Workbook.Worksheets.Add("ActressAdultLinks");
        actressLinksSheet.Cells[1, 1].Value = "ActressAdultId";
        actressLinksSheet.Cells[1, 2].Value = "ActressAdultName";
        actressLinksSheet.Cells[1, 3].Value = "Url";
        actressLinksSheet.Cells[1, 4].Value = "OrderIndex";

        var actressLinksHeader = actressLinksSheet.Cells[1, 1, 1, 4];
        actressLinksHeader.Style.Font.Bold = true;
        actressLinksHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        actressLinksHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var link in actressLinks)
        {
            actressLinksSheet.Cells[row, 1].Value = link.ActressAdultId;
            actressLinksSheet.Cells[row, 2].Value = link.ActressAdultName;
            actressLinksSheet.Cells[row, 3].Value = link.Url;
            actressLinksSheet.Cells[row, 4].Value = link.OrderIndex;
            row++;
        }

        var videosSheet = package.Workbook.Worksheets.Add("Videos");
        videosSheet.Cells[1, 1].Value = "Id";
        videosSheet.Cells[1, 2].Value = "Source";
        videosSheet.Cells[1, 3].Value = "ExternalId";
        videosSheet.Cells[1, 4].Value = "VideoUrl";
        videosSheet.Cells[1, 5].Value = "Title";
        videosSheet.Cells[1, 6].Value = "ThumbnailUrl";
        videosSheet.Cells[1, 7].Value = "Status";
        videosSheet.Cells[1, 8].Value = "TagIds";
        videosSheet.Cells[1, 9].Value = "ActressIds";
        videosSheet.Cells[1, 10].Value = "ActressNames";

        var videosHeader = videosSheet.Cells[1, 1, 1, 10];
        videosHeader.Style.Font.Bold = true;
        videosHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        videosHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        videosHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        row = 2;
        foreach (var video in videos)
        {
            videosSheet.Cells[row, 1].Value = video.Id;
            videosSheet.Cells[row, 2].Value = video.Source;
            videosSheet.Cells[row, 3].Value = video.ExternalId;
            videosSheet.Cells[row, 4].Value = video.VideoUrl;
            videosSheet.Cells[row, 5].Value = video.Title;
            videosSheet.Cells[row, 6].Value = video.ThumbnailUrl;
            videosSheet.Cells[row, 7].Value = video.Status;
            videosSheet.Cells[row, 8].Value = video.TagIds;
            videosSheet.Cells[row, 9].Value = video.ActressIds;
            videosSheet.Cells[row, 10].Value = video.ActressNames;
            row++;
        }

        var videoLinksSheet = package.Workbook.Worksheets.Add("VideoLinks");
        videoLinksSheet.Cells[1, 1].Value = "VideoAdultId";
        videoLinksSheet.Cells[1, 2].Value = "VideoExternalId";
        videoLinksSheet.Cells[1, 3].Value = "Url";
        videoLinksSheet.Cells[1, 4].Value = "OrderIndex";

        var videoLinksHeader = videoLinksSheet.Cells[1, 1, 1, 4];
        videoLinksHeader.Style.Font.Bold = true;
        videoLinksHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        videoLinksHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var videoLink in videoLinks)
        {
            videoLinksSheet.Cells[row, 1].Value = videoLink.VideoAdultId;
            videoLinksSheet.Cells[row, 2].Value = videoLink.VideoExternalId;
            videoLinksSheet.Cells[row, 3].Value = videoLink.Url;
            videoLinksSheet.Cells[row, 4].Value = videoLink.OrderIndex;
            row++;
        }

        if (actressSheet.Dimension is not null)
            actressSheet.Cells[actressSheet.Dimension.Address].AutoFitColumns();
        if (actressLinksSheet.Dimension is not null)
            actressLinksSheet.Cells[actressLinksSheet.Dimension.Address].AutoFitColumns();
        if (videosSheet.Dimension is not null)
            videosSheet.Cells[videosSheet.Dimension.Address].AutoFitColumns();
        if (videoLinksSheet.Dimension is not null)
            videoLinksSheet.Cells[videoLinksSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static ActressAdultExcelData ReadActressAdultExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var actressSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressAdult")
                        ?? package.Workbook.Worksheets.FirstOrDefault();
        var actressLinksSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressAdultLinks");
        var videosSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Videos");
        var videoLinksSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "VideoLinks");

        var result = new ActressAdultExcelData();
        if (actressSheet?.Dimension is null)
            return result;

        for (int row = 2; row <= actressSheet.Dimension.End.Row; row++)
        {
            var idText = actressSheet.Cells[row, 1].Text?.Trim() ?? "0";
            var name = actressSheet.Dimension.End.Column >= 2
                ? actressSheet.Cells[row, 2].Text?.Trim() ?? string.Empty
                : actressSheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            var image = actressSheet.Dimension.End.Column >= 3
                ? actressSheet.Cells[row, 3].Text?.Trim()
                : actressSheet.Dimension.End.Column >= 2
                    ? actressSheet.Cells[row, 2].Text?.Trim()
                    : null;
            var tagIds = actressSheet.Dimension.End.Column >= 4
                ? actressSheet.Cells[row, 4].Text?.Trim()
                : null;

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(image))
                continue;

            int.TryParse(idText, out var id);

            result.Actresses.Add(new ActressAdultExcelRow
            {
                Id = id,
                Name = name,
                Image = image,
                TagIds = tagIds,
            });
        }

        if (actressLinksSheet?.Dimension is not null)
        {
            for (int row = 2; row <= actressLinksSheet.Dimension.End.Row; row++)
            {
                var actressIdText = actressLinksSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var actressName = actressLinksSheet.Cells[row, 2].Text?.Trim();
                var url = actressLinksSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var orderText = actressLinksSheet.Cells[row, 4].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(actressIdText) && string.IsNullOrWhiteSpace(actressName) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(actressIdText, out var actressId);
                int.TryParse(orderText, out var orderIndex);

                result.ActressLinks.Add(new ActressAdultLinkExcelRow
                {
                    ActressAdultId = actressId,
                    ActressAdultName = actressName,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        if (videosSheet?.Dimension is not null)
        {
            for (int row = 2; row <= videosSheet.Dimension.End.Row; row++)
            {
                var idText = videosSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var source = videosSheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var externalId = videosSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var videoUrl = videosSheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
                var title = videosSheet.Cells[row, 5].Text?.Trim();
                var thumbnailUrl = videosSheet.Cells[row, 6].Text?.Trim();
                var statusText = videosSheet.Cells[row, 7].Text?.Trim() ?? "0";
                var tagIds = videosSheet.Dimension.End.Column >= 8 ? videosSheet.Cells[row, 8].Text?.Trim() : null;
                var actressIds = videosSheet.Dimension.End.Column >= 9 ? videosSheet.Cells[row, 9].Text?.Trim() : null;
                var actressNames = videosSheet.Dimension.End.Column >= 10 ? videosSheet.Cells[row, 10].Text?.Trim() : null;

                if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(externalId) && string.IsNullOrWhiteSpace(videoUrl))
                    continue;

                int.TryParse(idText, out var id);
                int.TryParse(statusText, out var status);

                result.Videos.Add(new VideoAdultExcelRow
                {
                    Id = id,
                    Source = source,
                    ExternalId = externalId,
                    VideoUrl = videoUrl,
                    Title = title,
                    ThumbnailUrl = thumbnailUrl,
                    Status = status,
                    TagIds = tagIds,
                    ActressIds = actressIds,
                    ActressNames = actressNames,
                });
            }
        }

        if (videoLinksSheet?.Dimension is not null)
        {
            for (int row = 2; row <= videoLinksSheet.Dimension.End.Row; row++)
            {
                var videoIdText = videoLinksSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var videoExternalId = videoLinksSheet.Cells[row, 2].Text?.Trim();
                var url = videoLinksSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var orderText = videoLinksSheet.Cells[row, 4].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(videoIdText) && string.IsNullOrWhiteSpace(videoExternalId) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(videoIdText, out var videoId);
                int.TryParse(orderText, out var orderIndex);

                result.VideoLinks.Add(new VideoAdultLinkExcelRow
                {
                    VideoAdultId = videoId,
                    VideoExternalId = videoExternalId,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        return result;
    }

    public static MemoryStream CreateAnimeGaleryExcel(List<AnimeGalery> galeries, List<GaleryMediaExcelRow> mediaRows, List<GaleryLinkExcelRow> linkRows)
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

        var linkSheet = package.Workbook.Worksheets.Add("AnimeGaleryLinks");
        linkSheet.Cells[1, 1].Value = "AnimeGaleryId";
        linkSheet.Cells[1, 2].Value = "Name";
        linkSheet.Cells[1, 3].Value = "Url";
        linkSheet.Cells[1, 4].Value = "OrderIndex";

        var linkHeader = linkSheet.Cells[1, 1, 1, 4];
        linkHeader.Style.Font.Bold = true;
        linkHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        linkHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var link in linkRows)
        {
            linkSheet.Cells[row, 1].Value = link.GaleryId;
            linkSheet.Cells[row, 2].Value = link.Name;
            linkSheet.Cells[row, 3].Value = link.Url;
            linkSheet.Cells[row, 4].Value = link.OrderIndex;
            row++;
        }

        if (linkSheet.Dimension is not null)
            linkSheet.Cells[linkSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static GaleryExcelData ReadAnimeGaleryExcel(Stream excelStream)
    {
        return ReadGaleryExcel(excelStream, "AnimeGaleries", "AnimeGaleryMedia", "AnimeGaleryLinks", "AnimeGaleryId");
    }

    public static MemoryStream CreateGirlGaleryExcel(List<GirlGalery> galeries, List<GaleryMediaExcelRow> mediaRows, List<GaleryLinkExcelRow> linkRows)
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

        var linkSheet = package.Workbook.Worksheets.Add("GirlGaleryLinks");
        linkSheet.Cells[1, 1].Value = "GirlGaleryId";
        linkSheet.Cells[1, 2].Value = "Name";
        linkSheet.Cells[1, 3].Value = "Url";
        linkSheet.Cells[1, 4].Value = "OrderIndex";

        var linkHeader = linkSheet.Cells[1, 1, 1, 4];
        linkHeader.Style.Font.Bold = true;
        linkHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        linkHeader.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        row = 2;
        foreach (var link in linkRows)
        {
            linkSheet.Cells[row, 1].Value = link.GaleryId;
            linkSheet.Cells[row, 2].Value = link.Name;
            linkSheet.Cells[row, 3].Value = link.Url;
            linkSheet.Cells[row, 4].Value = link.OrderIndex;
            row++;
        }

        if (linkSheet.Dimension is not null)
            linkSheet.Cells[linkSheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static GaleryExcelData ReadGirlGaleryExcel(Stream excelStream)
    {
        return ReadGaleryExcel(excelStream, "GirlGaleries", "GirlGaleryMedia", "GirlGaleryLinks", "GirlGaleryId");
    }

    private static GaleryExcelData ReadGaleryExcel(Stream excelStream, string galerySheetName, string mediaSheetName, string linkSheetName, string mediaGaleryIdHeader)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var galerySheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == galerySheetName)
                         ?? package.Workbook.Worksheets.FirstOrDefault();
        var mediaSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == mediaSheetName);
        var linkSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == linkSheetName);

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

        if (linkSheet?.Dimension is not null)
        {
            var galeryIdColumn = 1;
            for (int col = 1; col <= linkSheet.Dimension.End.Column; col++)
            {
                if (string.Equals(linkSheet.Cells[1, col].Text?.Trim(), mediaGaleryIdHeader, StringComparison.OrdinalIgnoreCase))
                {
                    galeryIdColumn = col;
                    break;
                }
            }

            for (int row = 2; row <= linkSheet.Dimension.End.Row; row++)
            {
                var galeryIdText = linkSheet.Cells[row, galeryIdColumn].Text?.Trim() ?? "0";
                var name = linkSheet.Cells[row, 2].Text?.Trim();
                var url = linkSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var orderText = linkSheet.Cells[row, 4].Text?.Trim() ?? "0";

                if (string.IsNullOrWhiteSpace(galeryIdText) && string.IsNullOrWhiteSpace(url))
                    continue;

                int.TryParse(galeryIdText, out var galeryId);
                int.TryParse(orderText, out var orderIndex);

                result.Links.Add(new GaleryLinkExcelRow
                {
                    GaleryId = galeryId,
                    Name = name,
                    Url = url,
                    OrderIndex = orderIndex,
                });
            }
        }

        return result;
    }
    public static MemoryStream CreateAnimeExcel(List<AnimeExcelRow> rows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Anime");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "ApiId";
        worksheet.Cells[1, 3].Value = "Title";
        worksheet.Cells[1, 4].Value = "Image";
        worksheet.Cells[1, 5].Value = "Episodes";
        worksheet.Cells[1, 6].Value = "Status";

        var headerRange = worksheet.Cells[1, 1, 1, 6];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in rows)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.ApiId;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.Image;
            worksheet.Cells[row, 5].Value = item.Episodes;
            worksheet.Cells[row, 6].Value = item.Status;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<AnimeExcelRow> ReadAnimeExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Anime")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<AnimeExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var apiId = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var title = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
            var episodesText = worksheet.Cells[row, 5].Text?.Trim() ?? "0";
            var statusText = worksheet.Cells[row, 6].Text?.Trim() ?? "0";

            if (string.IsNullOrWhiteSpace(apiId) && string.IsNullOrWhiteSpace(title))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(episodesText, out var episodes);
            int.TryParse(statusText, out var status);

            result.Add(new AnimeExcelRow
            {
                Id = id,
                ApiId = apiId,
                Title = title,
                Image = image,
                Episodes = episodes,
                Status = status,
            });
        }

        return result;
    }

    public static MemoryStream CreateHentaiExcel(List<HentaiExcelRow> rows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Hentai");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "ApiId";
        worksheet.Cells[1, 3].Value = "Title";
        worksheet.Cells[1, 4].Value = "Image";
        worksheet.Cells[1, 5].Value = "Episodes";
        worksheet.Cells[1, 6].Value = "Status";
        worksheet.Cells[1, 7].Value = "TagIds";

        var headerRange = worksheet.Cells[1, 1, 1, 7];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in rows)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.ApiId;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.Image;
            worksheet.Cells[row, 5].Value = item.Episodes;
            worksheet.Cells[row, 6].Value = item.Status;
            worksheet.Cells[row, 7].Value = item.TagIds;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<HentaiExcelRow> ReadHentaiExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Hentai")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<HentaiExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var apiId = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var title = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
            var episodesText = worksheet.Cells[row, 5].Text?.Trim() ?? "0";
            var statusText = worksheet.Cells[row, 6].Text?.Trim() ?? "0";
            var tagIds = worksheet.Cells[row, 7].Text?.Trim();

            if (string.IsNullOrWhiteSpace(apiId) && string.IsNullOrWhiteSpace(title))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(episodesText, out var episodes);
            int.TryParse(statusText, out var status);

            result.Add(new HentaiExcelRow
            {
                Id = id,
                ApiId = apiId,
                Title = title,
                Image = image,
                Episodes = episodes,
                Status = status,
                TagIds = tagIds,
            });
        }

        return result;
    }

    public static MemoryStream CreateSeriesExcel(List<SeriesExcelRow> rows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Series");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "ImdbId";
        worksheet.Cells[1, 3].Value = "Title";
        worksheet.Cells[1, 4].Value = "Image";
        worksheet.Cells[1, 5].Value = "Year";
        worksheet.Cells[1, 6].Value = "Rating";
        worksheet.Cells[1, 7].Value = "Type";
        worksheet.Cells[1, 8].Value = "Status";

        var headerRange = worksheet.Cells[1, 1, 1, 8];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in rows)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.ImdbId;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.Image;
            worksheet.Cells[row, 5].Value = item.Year;
            worksheet.Cells[row, 6].Value = item.Rating;
            worksheet.Cells[row, 7].Value = item.Type;
            worksheet.Cells[row, 8].Value = item.Status;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<SeriesExcelRow> ReadSeriesExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Series")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<SeriesExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var imdbId = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var title = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
            var yearText = worksheet.Cells[row, 5].Text?.Trim();
            var ratingText = worksheet.Cells[row, 6].Text?.Trim();
            var type = worksheet.Cells[row, 7].Text?.Trim();
            var statusText = worksheet.Cells[row, 8].Text?.Trim() ?? "0";

            if (string.IsNullOrWhiteSpace(imdbId) && string.IsNullOrWhiteSpace(title))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(yearText, out var year);
            decimal.TryParse(ratingText, NumberStyles.Any, CultureInfo.InvariantCulture, out var rating);
            int.TryParse(statusText, out var status);

            result.Add(new SeriesExcelRow
            {
                Id = id,
                ImdbId = imdbId,
                Title = title,
                Image = image,
                Year = yearText is null ? null : year,
                Rating = string.IsNullOrWhiteSpace(ratingText) ? null : rating,
                Type = type,
                Status = status,
            });
        }

        return result;
    }

    public static MemoryStream CreateYouTubeExcel(List<YouTubeExcelRow> rows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("YouTube");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Url";
        worksheet.Cells[1, 3].Value = "Title";
        worksheet.Cells[1, 4].Value = "AuthorName";
        worksheet.Cells[1, 5].Value = "AuthorUrl";
        worksheet.Cells[1, 6].Value = "Type";
        worksheet.Cells[1, 7].Value = "Height";
        worksheet.Cells[1, 8].Value = "Width";
        worksheet.Cells[1, 9].Value = "Version";
        worksheet.Cells[1, 10].Value = "ProviderName";
        worksheet.Cells[1, 11].Value = "ProviderUrl";
        worksheet.Cells[1, 12].Value = "ThumbnailHeight";
        worksheet.Cells[1, 13].Value = "ThumbnailWidth";
        worksheet.Cells[1, 14].Value = "ThumbnailUrl";
        worksheet.Cells[1, 15].Value = "Html";
        worksheet.Cells[1, 16].Value = "Category";

        var headerRange = worksheet.Cells[1, 1, 1, 16];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in rows)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.Url;
            worksheet.Cells[row, 3].Value = item.Title;
            worksheet.Cells[row, 4].Value = item.AuthorName;
            worksheet.Cells[row, 5].Value = item.AuthorUrl;
            worksheet.Cells[row, 6].Value = item.Type;
            worksheet.Cells[row, 7].Value = item.Height;
            worksheet.Cells[row, 8].Value = item.Width;
            worksheet.Cells[row, 9].Value = item.Version;
            worksheet.Cells[row, 10].Value = item.ProviderName;
            worksheet.Cells[row, 11].Value = item.ProviderUrl;
            worksheet.Cells[row, 12].Value = item.ThumbnailHeight;
            worksheet.Cells[row, 13].Value = item.ThumbnailWidth;
            worksheet.Cells[row, 14].Value = item.ThumbnailUrl;
            worksheet.Cells[row, 15].Value = item.Html;
            worksheet.Cells[row, 16].Value = item.Category;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<YouTubeExcelRow> ReadYouTubeExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "YouTube")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<YouTubeExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var url = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var title = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var authorName = worksheet.Cells[row, 4].Text?.Trim();
            var authorUrl = worksheet.Cells[row, 5].Text?.Trim();
            var type = worksheet.Cells[row, 6].Text?.Trim();
            var heightText = worksheet.Cells[row, 7].Text?.Trim();
            var widthText = worksheet.Cells[row, 8].Text?.Trim();
            var version = worksheet.Cells[row, 9].Text?.Trim();
            var providerName = worksheet.Cells[row, 10].Text?.Trim();
            var providerUrl = worksheet.Cells[row, 11].Text?.Trim();
            var thumbnailHeightText = worksheet.Cells[row, 12].Text?.Trim();
            var thumbnailWidthText = worksheet.Cells[row, 13].Text?.Trim();
            var thumbnailUrl = worksheet.Cells[row, 14].Text?.Trim();
            var html = worksheet.Cells[row, 15].Text?.Trim();
            var categoryText = worksheet.Cells[row, 16].Text?.Trim() ?? "0";

            if (string.IsNullOrWhiteSpace(url) && string.IsNullOrWhiteSpace(title))
                continue;

            int.TryParse(idText, out var id);
            int.TryParse(heightText, out var height);
            int.TryParse(widthText, out var width);
            int.TryParse(thumbnailHeightText, out var thumbnailHeight);
            int.TryParse(thumbnailWidthText, out var thumbnailWidth);
            int.TryParse(categoryText, out var category);

            result.Add(new YouTubeExcelRow
            {
                Id = id,
                Url = url,
                Title = title,
                AuthorName = authorName,
                AuthorUrl = authorUrl,
                Type = type,
                Height = string.IsNullOrWhiteSpace(heightText) ? null : height,
                Width = string.IsNullOrWhiteSpace(widthText) ? null : width,
                Version = version,
                ProviderName = providerName,
                ProviderUrl = providerUrl,
                ThumbnailHeight = string.IsNullOrWhiteSpace(thumbnailHeightText) ? null : thumbnailHeight,
                ThumbnailWidth = string.IsNullOrWhiteSpace(thumbnailWidthText) ? null : thumbnailWidth,
                ThumbnailUrl = thumbnailUrl,
                Html = html,
                Category = category,
            });
        }

        return result;
    }

    public static MemoryStream CreateComicExcel(List<ComicExcelRow> rows)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Comic");

        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Image";
        worksheet.Cells[1, 4].Value = "Url";
        worksheet.Cells[1, 5].Value = "Category";

        var headerRange = worksheet.Cells[1, 1, 1, 5];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        int row = 2;
        foreach (var item in rows)
        {
            worksheet.Cells[row, 1].Value = item.Id;
            worksheet.Cells[row, 2].Value = item.Name;
            worksheet.Cells[row, 3].Value = item.Image;
            worksheet.Cells[row, 4].Value = item.Url;
            worksheet.Cells[row, 5].Value = item.Category;
            row++;
        }

        if (worksheet.Dimension is not null)
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    public static List<ComicExcelRow> ReadComicExcel(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Comic")
                        ?? package.Workbook.Worksheets.FirstOrDefault();

        var result = new List<ComicExcelRow>();
        if (worksheet?.Dimension is null)
            return result;

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var idText = worksheet.Cells[row, 1].Text?.Trim() ?? "0";
            var name = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
            var image = worksheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
            var url = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty;
            var category = worksheet.Cells[row, 5].Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
                continue;

            int.TryParse(idText, out var id);

            result.Add(new ComicExcelRow
            {
                Id = id,
                Name = name,
                Image = image,
                Url = url,
                Category = category,
            });
        }

        return result;
    }
}