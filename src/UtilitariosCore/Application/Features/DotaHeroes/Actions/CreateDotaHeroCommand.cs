using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaHeroes.Actions;

public record CreateDotaHeroCommand(string Name, string? Image) : IRequest<Result<int>>;

public class CreateDotaHeroCommandValidator : AbstractValidator<CreateDotaHeroCommand>
{
    public CreateDotaHeroCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
}

internal sealed class CreateDotaHeroCommandHandler(IDotaHeroRepository repository)
    : IRequestHandler<CreateDotaHeroCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDotaHeroCommand request, CancellationToken cancellationToken)
    {
        var hero = new DotaHero { Name = request.Name, Image = request.Image, CreatedAt = DateTime.Now };
        return await repository.Create(hero);
    }
}
