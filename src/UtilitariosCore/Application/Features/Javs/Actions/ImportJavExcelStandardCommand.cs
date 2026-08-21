using MediatR;
using OfficeOpenXml;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record ImportJavExcelStandardCommand : IRequest<Result<ImportJavExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportJavExcelStandardCommandHandler(
    IJavRepository javRepository,
    IActressJavRepository actressRepository,
    ILinkJavRepository linkJavRepository,
    ILinkActressJavRepository linkActressJavRepository)
    : IRequestHandler<ImportJavExcelStandardCommand, Result<ImportJavExcelResult>>
{
    public async Task<Result<ImportJavExcelResult>> Handle(ImportJavExcelStandardCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var stream = new MemoryStream(request.FileBytes);
        using var package = new ExcelPackage(stream);

        var wsJavs = package.Workbook.Worksheets["Javs"];
        var wsActress = package.Workbook.Worksheets["ActressJav"];
        var wsJavLinks = package.Workbook.Worksheets["JavLinks"];
        var wsActressLinks = package.Workbook.Worksheets["ActressJavLinks"];
        var wsRelations = package.Workbook.Worksheets["Relations"];

        if (wsJavs is null || wsJavs.Dimension is null)
            return Errors.BadRequest("La hoja 'Javs' es requerida y no puede estar vacia.");

        var result = new ImportJavExcelResult();

        var allJavs = (await javRepository.GetAllJavs()).ToList();
        var javByCode = allJavs.ToDictionary(
            j => j.Code.ToUpperInvariant(),
            j => j,
            StringComparer.OrdinalIgnoreCase);

        var allActresses = (await actressRepository.GetAllActressJav()).ToList();
        var actressByCanonical = allActresses.ToDictionary(
            a => StringNormalizer.GetCanonicalFormForComparison(a.Name),
            a => a,
            StringComparer.OrdinalIgnoreCase);

        if (!await ImportJavsSheet(wsJavs, javByCode, result, cancellationToken))
            return Errors.BadRequest("No se pudo importar la hoja 'Javs'.");

        if (wsActress?.Dimension is not null)
            if (!await ImportActressSheet(wsActress, actressByCanonical, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'ActressJav'.");

        if (wsJavLinks?.Dimension is not null)
            if (!await ImportJavLinksSheet(wsJavLinks, javByCode, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'JavLinks'.");

        if (wsActressLinks?.Dimension is not null)
            if (!await ImportActressLinksSheet(wsActressLinks, actressByCanonical, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'ActressJavLinks'.");

        if (wsRelations?.Dimension is not null)
            if (!await ImportRelationsSheet(wsRelations, javByCode, actressByCanonical, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'Relations'.");

        return result;
    }

    private async Task<bool> ImportJavsSheet(
        ExcelWorksheet ws,
        Dictionary<string, Jav> javByCode,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int codeCol = GetColumnIndex(ws, "Code");
        int imageCol = GetOptionalColumnIndex(ws, "Image");
        int statusCol = GetOptionalColumnIndex(ws, "Status");
        int lastRow = ws.Dimension!.End.Row;

        for (int row = 2; row <= lastRow; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rawCode = ws.Cells[row, codeCol].Text?.Trim();
            if (string.IsNullOrWhiteSpace(rawCode))
            {
                result.Invalid++;
                continue;
            }

            var code = rawCode.ToUpperInvariant();
            if (javByCode.ContainsKey(code))
            {
                result.Skipped++;
                continue;
            }

            var image = imageCol > 0 ? ws.Cells[row, imageCol].Text?.Trim() ?? string.Empty : string.Empty;
            var status = ContentStatus.Pending;
            if (statusCol > 0 && int.TryParse(ws.Cells[row, statusCol].Text?.Trim(), out var statusInt) && Enum.IsDefined(typeof(ContentStatus), statusInt))
            {
                status = (ContentStatus)statusInt;
            }

            var jav = new Jav
            {
                Code = code,
                Image = image,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            var newId = await javRepository.CreateJav(jav);
            jav.Id = newId;
            javByCode[code] = jav;
            result.JavsCreated++;
        }

        return true;
    }

    private async Task<bool> ImportActressSheet(
        ExcelWorksheet ws,
        Dictionary<string, ActressJav> actressByCanonical,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int nameCol = GetColumnIndex(ws, "Name");
        int imageCol = GetOptionalColumnIndex(ws, "Image");
        int lastRow = ws.Dimension!.End.Row;

        for (int row = 2; row <= lastRow; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rawName = ws.Cells[row, nameCol].Text?.Trim();
            if (string.IsNullOrWhiteSpace(rawName))
            {
                result.Invalid++;
                continue;
            }

            var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);
            if (string.IsNullOrWhiteSpace(canonical))
            {
                result.Invalid++;
                continue;
            }

            if (actressByCanonical.ContainsKey(canonical))
            {
                result.Skipped++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(rawName);
            var image = imageCol > 0 ? ws.Cells[row, imageCol].Text?.Trim() : null;

            var actress = new ActressJav
            {
                Name = normalizedName,
                Image = string.IsNullOrWhiteSpace(image) ? null : image,
                CreatedAt = DateTime.UtcNow
            };

            var newId = await actressRepository.CreateActressJav(actress);
            actress.Id = newId;
            actressByCanonical[canonical] = actress;
            result.ActressesCreated++;
        }

        return true;
    }

    private async Task<bool> ImportJavLinksSheet(
        ExcelWorksheet ws,
        Dictionary<string, Jav> javByCode,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int codeCol = GetColumnIndex(ws, "Code");
        int linkCol = GetColumnIndex(ws, "Link");
        int orderCol = GetOptionalColumnIndex(ws, "OrderIndex");
        int lastRow = ws.Dimension!.End.Row;

        var existingByJavId = new Dictionary<int, HashSet<string>>();

        for (int row = 2; row <= lastRow; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rawCode = ws.Cells[row, codeCol].Text?.Trim();
            var rawUrl = ws.Cells[row, linkCol].Text?.Trim();

            if (string.IsNullOrWhiteSpace(rawCode) || string.IsNullOrWhiteSpace(rawUrl))
            {
                result.Invalid++;
                continue;
            }

            var code = rawCode.ToUpperInvariant();
            if (!javByCode.TryGetValue(code, out var jav))
            {
                result.Invalid++;
                continue;
            }

            if (!existingByJavId.TryGetValue(jav.Id, out var urlSet))
            {
                var existingLinks = await linkJavRepository.GetLinkJavsByJavId(jav.Id);
                urlSet = existingLinks
                    .Select(l => l.Url.Trim())
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                existingByJavId[jav.Id] = urlSet;
            }

            if (!urlSet.Add(rawUrl))
            {
                result.Skipped++;
                continue;
            }

            var orderIndex = orderCol > 0 && int.TryParse(ws.Cells[row, orderCol].Text?.Trim(), out var order) ? order : (int?)null;

            await linkJavRepository.CreateLinkJav(new LinkJav
            {
                JavId = jav.Id,
                Url = rawUrl,
                OrderIndex = orderIndex,
                CreatedAt = DateTime.UtcNow
            });
        }

        return true;
    }

    private async Task<bool> ImportActressLinksSheet(
        ExcelWorksheet ws,
        Dictionary<string, ActressJav> actressByCanonical,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int nameCol = GetColumnIndex(ws, "ActressName");
        int linkCol = GetColumnIndex(ws, "Link");
        int orderCol = GetOptionalColumnIndex(ws, "OrderIndex");
        int lastRow = ws.Dimension!.End.Row;

        var existingByActressId = new Dictionary<int, HashSet<string>>();

        for (int row = 2; row <= lastRow; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rawName = ws.Cells[row, nameCol].Text?.Trim();
            var rawUrl = ws.Cells[row, linkCol].Text?.Trim();

            if (string.IsNullOrWhiteSpace(rawName) || string.IsNullOrWhiteSpace(rawUrl))
            {
                result.Invalid++;
                continue;
            }

            var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);
            if (!actressByCanonical.TryGetValue(canonical, out var actress))
            {
                result.Invalid++;
                continue;
            }

            if (!existingByActressId.TryGetValue(actress.Id, out var urlSet))
            {
                var existingLinks = await linkActressJavRepository.GetLinkActressJavsByActressId(actress.Id);
                urlSet = existingLinks
                    .Select(l => l.Url.Trim())
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                existingByActressId[actress.Id] = urlSet;
            }

            if (!urlSet.Add(rawUrl))
            {
                result.Skipped++;
                continue;
            }

            var orderIndex = orderCol > 0 && int.TryParse(ws.Cells[row, orderCol].Text?.Trim(), out var order) ? order : (int?)null;

            await linkActressJavRepository.CreateLinkActressJav(new LinkActressJav
            {
                ActressJavId = actress.Id,
                Url = rawUrl,
                OrderIndex = orderIndex,
                CreatedAt = DateTime.UtcNow
            });
        }

        return true;
    }

    private async Task<bool> ImportRelationsSheet(
        ExcelWorksheet ws,
        Dictionary<string, Jav> javByCode,
        Dictionary<string, ActressJav> actressByCanonical,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int codeCol = GetColumnIndex(ws, "Code");
        int nameCol = GetColumnIndex(ws, "ActressName");
        int lastRow = ws.Dimension!.End.Row;

        var existingRelationsByJav = new Dictionary<int, HashSet<int>>();

        for (int row = 2; row <= lastRow; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rawCode = ws.Cells[row, codeCol].Text?.Trim();
            var rawName = ws.Cells[row, nameCol].Text?.Trim();

            if (string.IsNullOrWhiteSpace(rawCode) || string.IsNullOrWhiteSpace(rawName))
            {
                result.Invalid++;
                continue;
            }

            var code = rawCode.ToUpperInvariant();
            if (!javByCode.TryGetValue(code, out var jav))
            {
                result.Invalid++;
                continue;
            }

            var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);
            if (!actressByCanonical.TryGetValue(canonical, out var actress))
            {
                result.Invalid++;
                continue;
            }

            if (!existingRelationsByJav.TryGetValue(jav.Id, out var actressIds))
            {
                var existingIds = await javRepository.GetActressIdsByJavId(jav.Id);
                actressIds = existingIds.ToHashSet();
                existingRelationsByJav[jav.Id] = actressIds;
            }

            if (!actressIds.Add(actress.Id))
            {
                result.Skipped++;
                continue;
            }

            await javRepository.AddActressToJav(jav.Id, actress.Id);
        }

        return true;
    }

    private static int GetColumnIndex(ExcelWorksheet ws, string headerName)
    {
        int maxCol = ws.Dimension?.End.Column ?? 0;
        for (int col = 1; col <= maxCol; col++)
        {
            var header = ws.Cells[1, col].Text?.Trim();
            if (string.Equals(header, headerName, StringComparison.OrdinalIgnoreCase))
                return col;
        }

        throw new InvalidOperationException($"No se encontro la columna requerida '{headerName}' en la hoja '{ws.Name}'.");
    }

    private static int GetOptionalColumnIndex(ExcelWorksheet ws, string headerName)
    {
        int maxCol = ws.Dimension?.End.Column ?? 0;
        for (int col = 1; col <= maxCol; col++)
        {
            var header = ws.Cells[1, col].Text?.Trim();
            if (string.Equals(header, headerName, StringComparison.OrdinalIgnoreCase))
                return col;
        }

        return -1; // Column not found, that's OK for optional columns
    }

}
