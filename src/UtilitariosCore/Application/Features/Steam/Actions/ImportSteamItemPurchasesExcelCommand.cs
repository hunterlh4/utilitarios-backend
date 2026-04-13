using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record ImportSteamItemPurchasesExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportSteamItemPurchasesExcelCommandHandler(ISteamRepository repository)
    : IRequestHandler<ImportSteamItemPurchasesExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportSteamItemPurchasesExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadSteamItemPurchasesExcel(stream);
        var orderedRows = rows
            .OrderBy(row => row.Id <= 0 ? int.MaxValue : row.Id)
            .ToList();

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in orderedRows)
        {
            if (row.SteamItemId <= 0 || row.PurchasePrice < 0 || row.SalePrice < 0)
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

            SteamItemPurchase? existing = null;
            if (row.Id > 0)
            {
                var purchaseExists = await repository.ExistsPurchase(row.Id);
                if (purchaseExists)
                {
                    existing = await repository.GetByIdPurchase(row.Id);
                }
            }

            var isSold = row.SalePrice > 0;
            decimal? profit = isSold ? row.SalePrice - row.PurchasePrice : null;
            var status = isSold ? PurchaseStatus.Vendido : PurchaseStatus.Comprado;
            DateTime? saleDate = isSold ? (row.SaleDate ?? DateTime.Now) : null;

            if (existing is not null)
            {
                existing.SteamItemId = row.SteamItemId;
                existing.PurchasePrice = row.PurchasePrice;
                existing.SalePrice = row.SalePrice;
                existing.Profit = profit;
                existing.Status = status;
                existing.PurchaseDate = row.PurchaseDate;
                existing.SaleDate = saleDate;

                await repository.UpdatePurchase(existing);
                updated++;

                continue;
            }

            await repository.CreatePurchase(new SteamItemPurchase
            {
                SteamItemId = row.SteamItemId,
                PurchasePrice = row.PurchasePrice,
                SalePrice = row.SalePrice,
                Profit = profit,
                Status = status,
                PurchaseDate = row.PurchaseDate == default ? DateTime.Now : row.PurchaseDate,
                SaleDate = saleDate,
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
