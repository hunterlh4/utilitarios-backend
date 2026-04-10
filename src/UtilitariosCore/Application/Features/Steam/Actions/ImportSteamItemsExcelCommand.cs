using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record ImportSteamItemsExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportSteamItemsExcelCommandHandler(ISteamRepository repository)
    : IRequestHandler<ImportSteamItemsExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportSteamItemsExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacío.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadSteamItemsExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Name) ||
                string.IsNullOrWhiteSpace(row.Image) ||
                string.IsNullOrWhiteSpace(row.MarketUrl) ||
                row.Price < 0 ||
                (row.Game != GameType.Dota2 && row.Game != GameType.CS2))
            {
                invalid++;
                continue;
            }

            SteamItem? existing = null;

            if (!string.IsNullOrWhiteSpace(row.ExternalId))
            {
                existing = await repository.GetItemByExternalIdAsync(row.ExternalId);
            }

            // Fallback: si no se encontro por ExternalId, buscar por Name+Game
            if (existing is null)
            {
                existing = await repository.GetItemByNameAndGameAsync(row.Name, (int)row.Game);
            }

            if (existing != null)
            {
                var nextExternalId = !string.IsNullOrWhiteSpace(row.ExternalId)
                    ? row.ExternalId
                    : existing.ExternalId;

                bool hasChanges = existing.ExternalId != nextExternalId ||
                                 existing.Name != row.Name ||
                                 existing.Image != row.Image ||
                                 existing.Price != row.Price ||
                                 existing.Game != row.Game ||
                                 existing.MarketUrl != row.MarketUrl;

                if (hasChanges)
                {
                    existing.ExternalId = nextExternalId;
                    existing.Name = row.Name;
                    existing.Image = row.Image;
                    existing.Price = row.Price;
                    existing.Game = row.Game;
                    existing.MarketUrl = row.MarketUrl;
                    existing.UpdatedAt = DateTime.Now;

                    await repository.UpdateItems(existing);
                    updated++;
                }
                else
                {
                    skipped++;
                }

                continue;
            }

            await repository.CreateItems(new SteamItem
            {
                ExternalId = row.ExternalId,
                Name = row.Name,
                Image = row.Image,
                Price = row.Price,
                Game = (GameType)row.Game,
                MarketUrl = row.MarketUrl,
                Status = SteamItemStatus.Historial,
                CreatedAt = DateTime.Now
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
