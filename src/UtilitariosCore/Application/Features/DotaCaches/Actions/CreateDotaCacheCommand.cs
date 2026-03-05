using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record CreateDotaCacheCommand(
    int TreasureId,
    int HeroId,
    string Name,
    string Photo,
    decimal? Price,
    int? Quantity,
    decimal? Total,
    string? Owner
) : IRequest<Result<int>>;

public class CreateDotaCacheCommandValidator : AbstractValidator<CreateDotaCacheCommand>
{
    public CreateDotaCacheCommandValidator()
    {
        RuleFor(x => x.TreasureId).GreaterThan(0);
        RuleFor(x => x.HeroId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Photo).NotEmpty();
    }
}

internal sealed class CreateDotaCacheCommandHandler(
    IDotaCacheRepository repository,
    IDotaTreasureRepository treasureRepository,
    IDotaHeroRepository heroRepository)
    : IRequestHandler<CreateDotaCacheCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDotaCacheCommand request, CancellationToken cancellationToken)
    {
        var treasureExists = await treasureRepository.Exists(request.TreasureId);
        if (!treasureExists) return Errors.NotFound("Cofre no encontrado.");

        var heroExists = await heroRepository.Exists(request.HeroId);
        if (!heroExists) return Errors.NotFound("Héroe no encontrado.");

        var cache = new DotaCache
        {
            TreasureId = request.TreasureId, HeroId = request.HeroId, Name = request.Name,
            Photo = request.Photo, Price = request.Price, Quantity = request.Quantity,
            Total = request.Total, Owner = request.Owner, CreatedAt = DateTime.Now
        };
        return await repository.Create(cache);
    }
}
