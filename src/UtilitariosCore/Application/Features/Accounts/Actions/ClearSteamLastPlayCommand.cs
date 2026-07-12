using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record ClearSteamLastPlayCommand : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<ClearSteamLastPlayCommand, Result>
    {
        public async Task<Result> Handle(ClearSteamLastPlayCommand request, CancellationToken ct)
        {
            await repo.ClearWeeklyLastPlay();
            return Results.NoContent();
        }
    }
}
