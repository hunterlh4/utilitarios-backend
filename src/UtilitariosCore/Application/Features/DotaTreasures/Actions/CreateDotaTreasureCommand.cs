using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.DotaTreasures.Actions;

public record CreateDotaTreasureCommand(
    string Name,
    string Image,
    string? ImagePresentation,
    int Year,
    TreasureType? Type
) : IRequest<Result<int>>;

public class CreateDotaTreasureCommandValidator : AbstractValidator<CreateDotaTreasureCommand>
{
    public CreateDotaTreasureCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.Year).GreaterThan(2000);
    }
}

internal sealed class CreateDotaTreasureCommandHandler(IDotaTreasureRepository repository)
    : IRequestHandler<CreateDotaTreasureCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDotaTreasureCommand request, CancellationToken cancellationToken)
    {
        var treasure = new DotaTreasure
        {
            Name = request.Name, Image = request.Image, ImagePresentation = request.ImagePresentation,
            Year = request.Year, Type = request.Type, CreatedAt = DateTime.Now
        };
        return await repository.Create(treasure);
    }
}
