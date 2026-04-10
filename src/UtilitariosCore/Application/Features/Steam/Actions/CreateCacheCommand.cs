using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaCaches.Actions;

public record CreateCacheCommand : IRequest<Result<int>>
{
    public int TreasureId { get; init; }
    public int HeroId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Photo { get; init; } = string.Empty;
    public decimal? Price { get; init; }
    public int? Quantity { get; init; }
    public decimal? Total { get; init; }
    public string? Owner { get; init; }
}

public class CreateDotaCacheCommandValidator : AbstractValidator<CreateCacheCommand>
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
    ISteamRepository repository)
    : IRequestHandler<CreateCacheCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCacheCommand request, CancellationToken cancellationToken)
    {
        var treasureExists = await repository.ExistsTreasure(request.TreasureId);
        if (!treasureExists) return Errors.NotFound("Cofre no encontrado.");

        var heroExists = await repository.ExistsHero(request.HeroId);
        if (!heroExists) return Errors.NotFound("Héroe no encontrado.");

        var cache = new DotaCache
        {
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
        return await repository.CreateCache(cache);
    }
}
