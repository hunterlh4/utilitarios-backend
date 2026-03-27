using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record BulkCreateSteamItemDto(
    string? ExternalId,
    string Name,
    string Image,
    decimal Price,
    GameType Game,
    string MarketUrl
);

public record BulkCreateSteamItemCommand(List<BulkCreateSteamItemDto> Items) : IRequest<Result<BulkCreateSteamItemResult>>;

public record BulkCreateSteamItemResult(int Created, int Skipped);

internal sealed class BulkCreateSteamItemCommandHandler(ISteamItemRepository repository)
    : IRequestHandler<BulkCreateSteamItemCommand, Result<BulkCreateSteamItemResult>>
{
    public async Task<Result<BulkCreateSteamItemResult>> Handle(BulkCreateSteamItemCommand request, CancellationToken cancellationToken)
    {
        int created = 0, skipped = 0;

        foreach (var dto in request.Items)
        {
            if (!string.IsNullOrEmpty(dto.ExternalId))
            {
                var exists = await repository.ExistsByExternalId(dto.ExternalId);
                if (exists) { skipped++; continue; }
            }

            await repository.Create(new SteamItem
            {
                ExternalId = dto.ExternalId,
                Name = dto.Name,
                Image = dto.Image,
                Price = dto.Price,
                Game = dto.Game,
                MarketUrl = dto.MarketUrl,
                Status = SteamItemStatus.PorComprar,
                CreatedAt = DateTime.Now
            });
            created++;
        }

        return Results.Created(new BulkCreateSteamItemResult(created, skipped));
    }
}
