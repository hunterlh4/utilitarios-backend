using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record DeleteGitHubAccountCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<DeleteGitHubAccountCommand, Result>
    {
        public async Task<Result> Handle(DeleteGitHubAccountCommand r, CancellationToken ct)
        {
            await repo.DeleteGitHub(r.Id);
            return Results.NoContent();
        }
    }
}
