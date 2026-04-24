using MediatR;
using OfficeOpenXml;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record ImportJavExcelTemporalCommand : IRequest<Result<ImportJavExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

public class ImportJavExcelResult
{
    public int JavsCreated { get; set; }
    public int ActressesCreated { get; set; }
    public int Skipped { get; set; }
    public int Invalid { get; set; }
}

internal sealed class ImportJavExcelTemporalCommandHandler(
    IJavRepository javRepository,
    IActressJavRepository actressRepository)
    : IRequestHandler<ImportJavExcelTemporalCommand, Result<ImportJavExcelResult>>
{
    public async Task<Result<ImportJavExcelResult>> Handle(ImportJavExcelTemporalCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacío.");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var stream = new MemoryStream(request.FileBytes);
        using var package = new ExcelPackage(stream);
        var ws = package.Workbook.Worksheets[0];

        if (ws == null || ws.Dimension == null)
            return Errors.BadRequest("El archivo Excel está vacío o no tiene hojas.");

        var result = new ImportJavExcelResult();

        // Cargar actrices existentes para deduplicación por nombre canónico
        var existingActresses = await actressRepository.GetAllActressJav();
        var actressByCanonical = existingActresses.ToDictionary(
            a => StringNormalizer.GetCanonicalFormForComparison(a.Name),
            a => a,
            StringComparer.OrdinalIgnoreCase);

        // Cargar JAVs existentes para deduplicación por código
        var existingJavs = await javRepository.GetAllJavs();
        var javByCode = existingJavs.ToDictionary(
            j => j.Code.ToUpperInvariant(),
            j => j,
            StringComparer.OrdinalIgnoreCase);

        int lastRow = ws.Dimension.End.Row;

        for (int row = 2; row <= lastRow; row++)
        {
            var codeRaw = ws.Cells[row, 1].Text?.Trim();
            var actressNamesRaw = ws.Cells[row, 2].Text?.Trim();

            // Ambas columnas vacías → skip
            if (string.IsNullOrWhiteSpace(codeRaw) && string.IsNullOrWhiteSpace(actressNamesRaw))
                continue;

            int? javId = null;

            // Procesar código JAV
            if (!string.IsNullOrWhiteSpace(codeRaw))
            {
                var code = codeRaw.ToUpperInvariant();

                if (!javByCode.TryGetValue(code, out var existingJav))
                {
                    javId = await javRepository.CreateJav(new Jav
                    {
                        Code = code,
                        Image = string.Empty,
                        Status = ContentStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    });
                    javByCode[code] = new Jav { Id = javId.Value, Code = code, Image = string.Empty };
                    result.JavsCreated++;
                }
                else
                {
                    javId = existingJav.Id;
                    result.Skipped++;
                }
            }

            // Procesar actrices (separadas por coma)
            if (!string.IsNullOrWhiteSpace(actressNamesRaw))
            {
                var names = actressNamesRaw
                    .Split(',')
                    .Select(n => n.Trim())
                    .Where(n => !string.IsNullOrWhiteSpace(n));

                foreach (var rawName in names)
                {
                    var normalized = StringNormalizer.ToTitleCaseWithNumbers(rawName);
                    var canonical = StringNormalizer.GetCanonicalFormForComparison(rawName);

                    int actressId;
                    if (!actressByCanonical.TryGetValue(canonical, out var existingActress))
                    {
                        actressId = await actressRepository.CreateActressJav(new ActressJav
                        {
                            Name = normalized,
                            CreatedAt = DateTime.UtcNow
                        });
                        var newActress = new ActressJav { Id = actressId, Name = normalized };
                        actressByCanonical[canonical] = newActress;
                        result.ActressesCreated++;
                    }
                    else
                    {
                        actressId = existingActress.Id;
                    }

                    // Vincular actriz al JAV si hay código
                    if (javId.HasValue)
                    {
                        await javRepository.AddActressToJav(javId.Value, actressId);
                    }
                }
            }
        }

        return result;
    }
}
