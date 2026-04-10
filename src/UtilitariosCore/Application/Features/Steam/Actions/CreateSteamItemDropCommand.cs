using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Steam.Actions;

public record CreateItemDropCommand : IRequest<Result<int>>

{
    public int SteamItemId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal SalePrice { get; init; }


    public class Validator : AbstractValidator<CreateItemDropCommand>
    {
        public Validator()
        {
            RuleFor(x => x.SteamItemId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        }
    }

    internal sealed class Handler(
        ISteamRepository repository,
        ISteamRepository steamItemRepository)
        : IRequestHandler<CreateItemDropCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateItemDropCommand request, CancellationToken cancellationToken)
        {
            var itemExists = await steamItemRepository.ExistsItems(request.SteamItemId);
            if (!itemExists) return Errors.NotFound("Steam item no encontrado.");

            var drop = new SteamItemDrop
            {
                SteamItemId = request.SteamItemId,
                Quantity = request.Quantity,
                Price = request.Price,
                SalePrice = request.SalePrice,
                Total = request.Quantity * request.SalePrice,
                CreatedAt = DateTime.Now
            };
            return await repository.CreateDrops(drop);
        }
    }
}