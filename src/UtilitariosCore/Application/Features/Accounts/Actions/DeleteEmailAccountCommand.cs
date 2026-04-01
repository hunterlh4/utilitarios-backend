using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record DeleteEmailAccountCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<DeleteEmailAccountCommand, Result>
    {
        public async Task<Result> Handle(DeleteEmailAccountCommand r, CancellationToken ct)
        {
            await repo.DeleteEmail(r.Id);
            return Results.NoContent();
        }
    }
}
