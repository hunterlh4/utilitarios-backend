using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record DeleteSteamAccountCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<DeleteSteamAccountCommand, Result>
    {
        public async Task<Result> Handle(DeleteSteamAccountCommand r, CancellationToken ct)
        {
            await repo.DeleteSteam(r.Id);
            return Results.NoContent();
        }
    }
}
