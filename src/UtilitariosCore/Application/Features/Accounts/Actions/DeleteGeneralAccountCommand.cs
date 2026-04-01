using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record DeleteGeneralAccountCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<DeleteGeneralAccountCommand, Result>
    {
        public async Task<Result> Handle(DeleteGeneralAccountCommand r, CancellationToken ct)
        {
            await repo.DeleteGeneral(r.Id);
            return Results.NoContent();
        }
    }
}
