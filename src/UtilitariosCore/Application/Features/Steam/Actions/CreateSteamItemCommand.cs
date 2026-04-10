using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record CreateSteamItemCommand : IRequest<Result<int>>
{
    public string? ExternalId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public GameType Game { get; init; }
    public string MarketUrl { get; init; } = string.Empty;


    public class Validator : AbstractValidator<CreateSteamItemCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.MarketUrl).NotEmpty();
        }
    }

    internal sealed class CreateSteamItemCommandHandler(ISteamRepository repository)
        : IRequestHandler<CreateSteamItemCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateSteamItemCommand request, CancellationToken cancellationToken)
        {
            var existingByNameAndGame = await repository.GetItemByNameAndGameAsync(request.Name, (int)request.Game);

            // Si ya existe por nombre+juego, actualizar (incluye completar ExternalId faltante)
            if (existingByNameAndGame != null)
            {
                if (!string.IsNullOrWhiteSpace(request.ExternalId))
                {
                    var existingByExternalId = await repository.GetItemByExternalIdAsync(request.ExternalId);
                    if (existingByExternalId != null && existingByExternalId.Id != existingByNameAndGame.Id)
                        return Errors.BadRequest("El externalId ya pertenece a otro item.");
                }

                existingByNameAndGame.ExternalId = !string.IsNullOrWhiteSpace(request.ExternalId)
                    ? request.ExternalId
                    : existingByNameAndGame.ExternalId;
                existingByNameAndGame.Image = request.Image;
                existingByNameAndGame.Price = request.Price;
                existingByNameAndGame.MarketUrl = request.MarketUrl;
                existingByNameAndGame.UpdatedAt = DateTime.Now;

                await repository.UpdateItems(existingByNameAndGame);
                return existingByNameAndGame.Id;
            }

            if (!string.IsNullOrWhiteSpace(request.ExternalId))
            {
                var existsByExternalId = await repository.ExistsByExternalIdItems(request.ExternalId);
                if (existsByExternalId)
                    return Errors.BadRequest("El item ya existe en la base de datos.");
            }

            var item = new SteamItem
            {
                ExternalId = request.ExternalId,
                Name = request.Name,
                Image = request.Image,
                Price = request.Price,
                Game = request.Game,
                MarketUrl = request.MarketUrl,
                Status = SteamItemStatus.Historial,
                CreatedAt = DateTime.Now
            };
            return await repository.CreateItems(item);
        }
    }
}