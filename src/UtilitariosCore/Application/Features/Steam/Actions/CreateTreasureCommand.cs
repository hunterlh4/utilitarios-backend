using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Steam.Actions;

public record CreateTreasureCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public string? ImagePresentation { get; init; }
    public int Year { get; init; }
    public TreasureType? Type { get; init; }
}

public class Validator : AbstractValidator<CreateTreasureCommand>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.Year).GreaterThan(2000);
    }
}

internal sealed class Handler(ISteamRepository repository)
    : IRequestHandler<CreateTreasureCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTreasureCommand request, CancellationToken cancellationToken)
    {
        var treasure = new DotaTreasure
        {
            Name = request.Name, 
            Image = request.Image, 
            ImagePresentation = request.ImagePresentation,
            Year = request.Year, 
            Type = request.Type, 
            CreatedAt = DateTime.Now
        };
        return await repository.CreateTreasure(treasure);
    }
}
