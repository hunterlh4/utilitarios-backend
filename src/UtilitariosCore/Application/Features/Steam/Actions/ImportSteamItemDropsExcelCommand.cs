using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record ImportSteamItemDropsExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportSteamItemDropsExcelCommandHandler(ISteamRepository repository)
    : IRequestHandler<ImportSteamItemDropsExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportSteamItemDropsExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadSteamItemDropsExcel(stream);
        var orderedRows = rows
            .OrderBy(row => row.Id <= 0 ? int.MaxValue : row.Id)
            .ToList();

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in orderedRows)
        {
            if (row.SteamItemId <= 0 || row.Quantity <= 0 || row.Price < 0 || row.SalePrice < 0)
            {
                invalid++;
                continue;
            }

            var itemExists = await repository.ExistsItems(row.SteamItemId);
            if (!itemExists)
            {
                skipped++;
                continue;
            }

            SteamItemDrop? existing = null;
            if (row.Id > 0)
            {
                var dropExists = await repository.ExistsDrops(row.Id);
                if (dropExists)
                {
                    existing = await repository.GetByIdDrops(row.Id);
                }
            }

            if (existing is not null)
            {
                existing.SteamItemId = row.SteamItemId;
                existing.Quantity = row.Quantity;
                existing.Price = row.Price;
                existing.SalePrice = row.SalePrice;
                existing.Total = row.Quantity * row.SalePrice;

                await repository.UpdateDrops(existing);
                updated++;

                continue;
            }

            await repository.CreateDrops(new SteamItemDrop
            {
                SteamItemId = row.SteamItemId,
                Quantity = row.Quantity,
                Price = row.Price,
                SalePrice = row.SalePrice,
                Total = row.Quantity * row.SalePrice,
                CreatedAt = row.CreatedAt == default ? DateTime.Now : row.CreatedAt,
            });

            created++;
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }
}
