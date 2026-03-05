using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record CreateSteamItemCommand(
    string Name,
    string Image,
    string? Price,
    GameType Game,
    string MarketUrl,
    SteamItemStatus Status
) : IRequest<Result<int>>;

public class CreateSteamItemCommandValidator : AbstractValidator<CreateSteamItemCommand>
{
    public CreateSteamItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.MarketUrl).NotEmpty();
    }
}

internal sealed class CreateSteamItemCommandHandler(ISteamItemRepository repository)
    : IRequestHandler<CreateSteamItemCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateSteamItemCommand request, CancellationToken cancellationToken)
    {
        var item = new SteamItem
        {
            Name = request.Name, Image = request.Image, Price = request.Price,
            Game = request.Game, MarketUrl = request.MarketUrl,
            Status = request.Status, CreatedAt = DateTime.Now
        };
        return await repository.Create(item);
    }
}
