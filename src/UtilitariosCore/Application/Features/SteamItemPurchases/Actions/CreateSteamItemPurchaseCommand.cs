using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record CreateSteamItemPurchaseCommand(
    int SteamItemId,
    decimal PurchasePrice,
    decimal SalePrice,
    decimal? Profit,
    PurchaseStatus Status,
    DateTime PurchaseDate,
    DateTime? SaleDate
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
            SalePrice = request.SalePrice,
            Profit = request.Profit,
            Status = request.Status,
            PurchaseDate = request.PurchaseDate,
            SaleDate = request.SaleDate,
            CreatedAt = DateTime.Now
        };
        return await repository.Create(purchase);
    }
}
