using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record CreateSteamItemDropCommand(
    int SteamItemId,
    int Quantity,
    decimal Price,
    decimal SalePrice,
    decimal Total
) : IRequest<Result<int>>;

public class CreateSteamItemDropCommandValidator : AbstractValidator<CreateSteamItemDropCommand>
{
    public CreateSteamItemDropCommandValidator()
    {
        RuleFor(x => x.SteamItemId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Total).GreaterThanOrEqualTo(0);
    }
}

internal sealed class CreateSteamItemDropCommandHandler(
    ISteamItemDropRepository repository,
    ISteamItemRepository steamItemRepository)
    : IRequestHandler<CreateSteamItemDropCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateSteamItemDropCommand request, CancellationToken cancellationToken)
    {
        var itemExists = await steamItemRepository.Exists(request.SteamItemId);
        if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

        var drop = new SteamItemDrop
        {
            SteamItemId = request.SteamItemId,
            Quantity = request.Quantity,
            Price = request.Price,
            SalePrice = request.SalePrice,
            Total = request.Total,
            CreatedAt = DateTime.Now
        };
        return await repository.Create(drop);
    }
}
