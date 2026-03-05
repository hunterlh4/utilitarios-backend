using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record UpdateDotaCacheCommand(
    int Id,
    int TreasureId,
    int HeroId,
    string Name,
    string Photo,
    decimal? Price,
    int? Quantity,
    decimal? Total,
    string? Owner
) : IRequest<Result>;

internal sealed class UpdateDotaCacheCommandHandler(
    IDotaCacheRepository repository,
    IDotaTreasureRepository treasureRepository,
    IDotaHeroRepository heroRepository)
    : IRequestHandler<UpdateDotaCacheCommand, Result>
{
    public async Task<Result> Handle(UpdateDotaCacheCommand request, CancellationToken cancellationToken)
    {
        var exists = await repository.Exists(request.Id);
        if (!exists) return Errors.NotFound("Cache no encontrado.");

        var treasureExists = await treasureRepository.Exists(request.TreasureId);
        if (!treasureExists) return Errors.NotFound("Cofre no encontrado.");

        var heroExists = await heroRepository.Exists(request.HeroId);
        if (!heroExists) return Errors.NotFound("Héroe no encontrado.");

        var cache = new Domain.Models.DotaCache
        {
            Id = request.Id, TreasureId = request.TreasureId, HeroId = request.HeroId,
            Name = request.Name, Photo = request.Photo, Price = request.Price,
            Quantity = request.Quantity, Total = request.Total, Owner = request.Owner,
            CreatedAt = DateTime.Now
        };
        await repository.Update(cache);
        return Results.NoContent();
    }
}
