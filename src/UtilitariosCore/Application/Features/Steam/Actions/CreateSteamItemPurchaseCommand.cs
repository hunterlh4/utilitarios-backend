using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record CreateSteamItemPurchaseCommand : IRequest<Result<int>>
    
{
    public int SteamItemId { get; set; }
    public decimal PurchasePrice { get; set; } = 0;
    public class Validator : AbstractValidator<CreateSteamItemPurchaseCommand>
    {
        public Validator()
        {
            RuleFor(x => x.SteamItemId).GreaterThan(0);
            RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
        }
    }

    internal sealed class CreateSteamItemPurchaseCommandHandler(
        ISteamRepository repository,
        ISteamRepository steamItemRepository)
        : IRequestHandler<CreateSteamItemPurchaseCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateSteamItemPurchaseCommand request, CancellationToken cancellationToken)
        {
            var itemExists = await steamItemRepository.ExistsItems(request.SteamItemId);
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
            return await repository.CreatePurchase(purchase);
        }
    }
}