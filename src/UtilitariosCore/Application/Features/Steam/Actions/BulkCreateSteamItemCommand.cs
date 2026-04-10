using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record BulkCreateSteamItemDto
{
    public string? ExternalId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public GameType Game { get; init; }
    public string MarketUrl { get; init; } = string.Empty;
}

public record BulkCreateSteamItemCommand : IRequest<Result<BulkCreateSteamItemResult>>
{
    public List<BulkCreateSteamItemDto> Items { get; init; } = [];
}

public record BulkCreateSteamItemResult
{
    public int Created { get; init; }
    public int Skipped { get; init; }
}

internal sealed class BulkCreateSteamItemCommandHandler(ISteamRepository repository)
    : IRequestHandler<BulkCreateSteamItemCommand, Result<BulkCreateSteamItemResult>>
{
    public async Task<Result<BulkCreateSteamItemResult>> Handle(BulkCreateSteamItemCommand request, CancellationToken cancellationToken)
    {
        int created = 0, skipped = 0;

        foreach (var dto in request.Items)
        {
            if (!string.IsNullOrEmpty(dto.ExternalId))
            {
                var exists = await repository.ExistsByExternalIdItems(dto.ExternalId);
                if (exists) { skipped++; continue; }
            }

            await repository.CreateItems(new SteamItem
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

        return Results.Created(new BulkCreateSteamItemResult { Created = created, Skipped = skipped });
    }
}
