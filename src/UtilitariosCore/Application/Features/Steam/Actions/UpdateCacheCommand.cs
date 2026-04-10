using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record UpdateCacheCommand : IRequest<Result>
{
    public int Id { get; init; }
    public int TreasureId { get; init; }
    public int HeroId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Photo { get; init; } = string.Empty;
    public decimal? Price { get; init; }
    public int? Quantity { get; init; }
    public decimal? Total { get; init; }
    public string? Owner { get; init; }
}

internal sealed class UpdateDotaCacheCommandHandler(
    ISteamRepository repository,
    ISteamRepository treasureRepository,
    ISteamRepository heroRepository)
    : IRequestHandler<UpdateCacheCommand, Result>
{
    public async Task<Result> Handle(UpdateCacheCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsCache(request.Id);
        if (!exists) return Errors.NotFound("Cache no encontrado.");

        var treasureExists = await treasureRepository.ExistsTreasure(request.TreasureId);
        if (!treasureExists) return Errors.NotFound("Cofre no encontrado.");

        var heroExists = await heroRepository.ExistsHero(request.HeroId);
        if (!heroExists) return Errors.NotFound("Héroe no encontrado.");

        var cache = new DotaCache
        {
            Id = request.Id, 
            TreasureId = request.TreasureId, 
            HeroId = request.HeroId,
            Name = request.Name, 
            Photo = request.Photo, 
            Price = request.Price,
            Quantity = request.Quantity, 
            Total = request.Total, 
            Owner = request.Owner,
            CreatedAt = DateTime.Now
        };
        await repository.UpdateCache(cache);
        return Results.NoContent();
    }
}
