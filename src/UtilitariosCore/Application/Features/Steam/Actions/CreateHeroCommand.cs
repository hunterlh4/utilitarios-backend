using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record CreateHeroCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
}

public class CreateHeroCommandValidator : AbstractValidator<CreateHeroCommand>
{
    public CreateHeroCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
}

internal sealed class CreateHeroCommandHandler(ISteamRepository repository)
    : IRequestHandler<CreateHeroCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = new DotaHero { 
            Name = request.Name, 
            Image = request.Image, 
            CreatedAt = DateTime.Now 
        };
        return await repository.CreateHero(hero);
    }
}
