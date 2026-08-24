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
    ILinkActressJavRepository linkActressJavRepository,
    ITagRepository tagRepository)
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

        // Obtener tags válidos para validación
        var validJavTagIds = (await tagRepository.GetAllTagsByType(TagType.Jav)).Select(tag => tag.Id).ToHashSet();
        var validActressTagIds = (await tagRepository.GetAllTagsByType(TagType.ActressJav)).Select(tag => tag.Id).ToHashSet();

        var allJavs = (await javRepository.GetAllJavs()).ToList();
        var javByCode = allJavs.ToDictionary(
            j => j.Code.ToUpperInvariant(),
            j => j,
            StringComparer.OrdinalIgnoreCase);
        var javById = allJavs.ToDictionary(j => j.Id);

        var allActresses = (await actressRepository.GetAllActressJav()).ToList();
        var actressByCanonical = allActresses.ToDictionary(
            a => StringNormalizer.GetCanonicalFormForComparison(a.Name),
            a => a,
            StringComparer.OrdinalIgnoreCase);
        var actressById = allActresses.ToDictionary(a => a.Id);

        if (!await ImportJavsSheet(wsJavs, javByCode, javById, validJavTagIds, result, cancellationToken))
            return Errors.BadRequest("No se pudo importar la hoja 'Javs'.");

        if (wsActress?.Dimension is not null)
            if (!await ImportActressSheet(wsActress, actressByCanonical, actressById, validActressTagIds, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'ActressJav'.");

        if (wsJavLinks?.Dimension is not null)
            if (!await ImportJavLinksSheet(wsJavLinks, javByCode, javById, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'JavLinks'.");

        if (wsActressLinks?.Dimension is not null)
            if (!await ImportActressLinksSheet(wsActressLinks, actressByCanonical, actressById, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'ActressJavLinks'.");

        if (wsRelations?.Dimension is not null)
            if (!await ImportRelationsSheet(wsRelations, javByCode, actressByCanonical, result, cancellationToken))
                return Errors.BadRequest("No se pudo importar la hoja 'Relations'.");

        return result;
    }

    private async Task<bool> ImportJavsSheet(
        ExcelWorksheet ws,
        Dictionary<string, Jav> javByCode,
        Dictionary<int, Jav> javById,
        HashSet<int> validJavTagIds,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int idCol = GetOptionalColumnIndex(ws, "Id");
        int codeCol = GetColumnIndex(ws, "Code");
        int imageCol = GetOptionalColumnIndex(ws, "Image");
        int statusCol = GetOptionalColumnIndex(ws, "Status");
        int tagIdsCol = GetOptionalColumnIndex(ws, "TagIds");
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
            Jav? existing = null;
            var wasCreated = false;
            var javUpdated = false;

            // Buscar por ID primero, luego por código
            if (idCol > 0 && int.TryParse(ws.Cells[row, idCol].Text?.Trim(), out var id) && id > 0)
            {
                javById.TryGetValue(id, out existing);
            }
            
            if (existing == null)
            {
                javByCode.TryGetValue(code, out existing);
            }

            var image = imageCol > 0 ? ws.Cells[row, imageCol].Text?.Trim() ?? string.Empty : string.Empty;
            var status = ContentStatus.Pending;
            if (statusCol > 0 && int.TryParse(ws.Cells[row, statusCol].Text?.Trim(), out var statusInt) && Enum.IsDefined(typeof(ContentStatus), statusInt))
            {
                status = (ContentStatus)statusInt;
            }

            if (existing == null)
            {
                // Crear nuevo
                var newId = await javRepository.CreateJav(new Jav
                {
                    Code = code,
                    Image = image,
                    Status = status,
                    CreatedAt = DateTime.UtcNow
                });

                existing = new Jav
                {
                    Id = newId,
                    Code = code,
                    Image = image,
                    Status = status,
                    CreatedAt = DateTime.UtcNow
                };

                javByCode[code] = existing;
                javById[newId] = existing;
                wasCreated = true;
            }
            else
            {
                // Actualizar existente
                var nextImage = string.IsNullOrWhiteSpace(image) ? existing.Image : image;
                var hasChanges = existing.Code != code || existing.Image != nextImage || existing.Status != status;

                if (hasChanges)
                {
                    existing.Code = code;
                    existing.Image = nextImage;
                    existing.Status = status;
                    await javRepository.UpdateJav(existing);
                    javByCode[code] = existing;
                    javById[existing.Id] = existing;
                    javUpdated = true;
                }
            }

            // Manejar tags si están presentes
            var hasTagChanges = false;
            if (tagIdsCol > 0)
            {
                var rawTagIds = ws.Cells[row, tagIdsCol].Text?.Trim();
                var desiredTagIds = ParseTagIds(rawTagIds, validJavTagIds, out var hasTagInput, out var hadInvalidTagTokens);
                if (hadInvalidTagTokens)
                    result.Invalid++;

                if (hasTagInput)
                {
                    var currentTagIds = (await tagRepository.GetTagsByRefId(existing.Id, TagType.Jav))
                        .Select(tag => tag.Id)
                        .ToHashSet();

                    if (!currentTagIds.SetEquals(desiredTagIds))
                    {
                        await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.Jav, desiredTagIds);
                        hasTagChanges = true;
                    }
                }
            }

            if (wasCreated)
                result.JavsCreated++;
            else if (javUpdated || hasTagChanges)
                result.Updated++;
            else
                result.Skipped++;
        }

        return true;
    }

    private async Task<bool> ImportActressSheet(
        ExcelWorksheet ws,
        Dictionary<string, ActressJav> actressByCanonical,
        Dictionary<int, ActressJav> actressById,
        HashSet<int> validActressTagIds,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int idCol = GetOptionalColumnIndex(ws, "Id");
        int nameCol = GetColumnIndex(ws, "Name");
        int imageCol = GetOptionalColumnIndex(ws, "Image");
        int tagIdsCol = GetOptionalColumnIndex(ws, "TagIds");
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

            var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(rawName);
            var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);
            ActressJav? existing = null;
            var wasCreated = false;
            var actressUpdated = false;

            // Buscar por ID primero, luego por nombre
            if (idCol > 0 && int.TryParse(ws.Cells[row, idCol].Text?.Trim(), out var id) && id > 0)
            {
                actressById.TryGetValue(id, out existing);
            }
            
            if (existing == null)
            {
                actressByCanonical.TryGetValue(canonical, out existing);
            }

            var image = imageCol > 0 ? ws.Cells[row, imageCol].Text?.Trim() : null;

            if (existing == null)
            {
                // Crear nueva
                var newId = await actressRepository.CreateActressJav(new ActressJav
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(image) ? null : image,
                    CreatedAt = DateTime.UtcNow
                });

                existing = new ActressJav
                {
                    Id = newId,
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(image) ? null : image,
                    CreatedAt = DateTime.UtcNow
                };

                actressByCanonical[canonical] = existing;
                actressById[newId] = existing;
                wasCreated = true;
            }
            else
            {
                // Actualizar existente
                var nextImage = string.IsNullOrWhiteSpace(image) ? existing.Image : image;
                var hasChanges = existing.Name != normalizedName || existing.Image != nextImage;

                if (hasChanges)
                {
                    existing.Name = normalizedName;
                    existing.Image = nextImage;
                    await actressRepository.UpdateActressJav(existing);
                    actressByCanonical[canonical] = existing;
                    actressById[existing.Id] = existing;
                    actressUpdated = true;
                }
            }

            // Manejar tags si están presentes
            var hasTagChanges = false;
            if (tagIdsCol > 0)
            {
                var rawTagIds = ws.Cells[row, tagIdsCol].Text?.Trim();
                var desiredTagIds = ParseTagIds(rawTagIds, validActressTagIds, out var hasTagInput, out var hadInvalidTagTokens);
                if (hadInvalidTagTokens)
                    result.Invalid++;

                if (hasTagInput)
                {
                    var currentTagIds = (await tagRepository.GetTagsByRefId(existing.Id, TagType.ActressJav))
                        .Select(tag => tag.Id)
                        .ToHashSet();

                    if (!currentTagIds.SetEquals(desiredTagIds))
                    {
                        await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.ActressJav, desiredTagIds);
                        hasTagChanges = true;
                    }
                }
            }

            if (wasCreated)
                result.ActressesCreated++;
            else if (actressUpdated || hasTagChanges)
                result.Updated++;
            else
                result.Skipped++;
        }

        return true;
    }

    private async Task<bool> ImportJavLinksSheet(
        ExcelWorksheet ws,
        Dictionary<string, Jav> javByCode,
        Dictionary<int, Jav> javById,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int javIdCol = GetOptionalColumnIndex(ws, "JavId");
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

            Jav? jav = null;
            
            // Buscar por ID primero, luego por código
            if (javIdCol > 0 && int.TryParse(ws.Cells[row, javIdCol].Text?.Trim(), out var javId) && javId > 0)
            {
                javById.TryGetValue(javId, out jav);
            }
            
            if (jav == null)
            {
                var code = rawCode.ToUpperInvariant();
                javByCode.TryGetValue(code, out jav);
            }

            if (jav == null)
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
        Dictionary<int, ActressJav> actressById,
        ImportJavExcelResult result,
        CancellationToken cancellationToken)
    {
        int actressIdCol = GetOptionalColumnIndex(ws, "ActressId");
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

            ActressJav? actress = null;
            
            // Buscar por ID primero, luego por nombre
            if (actressIdCol > 0 && int.TryParse(ws.Cells[row, actressIdCol].Text?.Trim(), out var actressId) && actressId > 0)
            {
                actressById.TryGetValue(actressId, out actress);
            }
            
            if (actress == null)
            {
                var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);
                actressByCanonical.TryGetValue(canonical, out actress);
            }

            if (actress == null)
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

    private static List<int> ParseTagIds(string? rawTags, HashSet<int> validTagIds, out bool hasInput, out bool hadInvalidTokens)
    {
        hasInput = !string.IsNullOrWhiteSpace(rawTags);
        hadInvalidTokens = false;

        if (!hasInput)
            return [];

        var tagIds = new HashSet<int>();
        var tokens = (rawTags ?? string.Empty).Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var token in tokens)
        {
            if (int.TryParse(token, out var tagId) && tagId > 0 && validTagIds.Contains(tagId))
            {
                tagIds.Add(tagId);
                continue;
            }

            hadInvalidTokens = true;
        }

        return tagIds.OrderBy(id => id).ToList();
    }

}
