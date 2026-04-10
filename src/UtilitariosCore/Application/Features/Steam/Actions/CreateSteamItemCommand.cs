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
            if (!string.IsNullOrEmpty(request.ExternalId))
            {
                var exists = await repository.ExistsByExternalIdItems(request.ExternalId);
                if (exists) return Errors.BadRequest("El item ya existe en la base de datos.");
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