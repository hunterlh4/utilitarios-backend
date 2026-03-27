using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record CreateSteamItemPurchaseCommand(
    int SteamItemId,
    decimal PurchasePrice
) : IRequest<Result<int>>;

public class CreateSteamItemPurchaseCommandValidator : AbstractValidator<CreateSteamItemPurchaseCommand>
{
    public CreateSteamItemPurchaseCommandValidator()
    {
        RuleFor(x => x.SteamItemId).GreaterThan(0);
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
    }
}

internal sealed class CreateSteamItemPurchaseCommandHandler(
    ISteamItemPurchaseRepository repository,
    ISteamItemRepository steamItemRepository)
    : IRequestHandler<CreateSteamItemPurchaseCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateSteamItemPurchaseCommand request, CancellationToken cancellationToken)
    {
        var itemExists = await steamItemRepository.Exists(request.SteamItemId);
        if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

        var purchase = new SteamItemPurchase
        {
            SteamItemId = request.SteamItemId,
            PurchasePrice = request.PurchasePrice,
            SalePrice = 0,
            Profit = null,
            Status = PurchaseStatus.Comprado,
            PurchaseDate = DateTime.Now,
            SaleDate = null,
            CreatedAt = DateTime.Now
        };
        return await repository.Create(purchase);
    }
}
