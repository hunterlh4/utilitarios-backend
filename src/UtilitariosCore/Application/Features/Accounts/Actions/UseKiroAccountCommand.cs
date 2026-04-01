using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UseKiroAccountCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UseKiroAccountCommand, Result>
    {
        public async Task<Result> Handle(UseKiroAccountCommand r, CancellationToken ct)
        {
            var updated = await repo.UseKiro(r.Id);
            if (!updated) return Errors.NotFound("Cuenta Kiro no encontrada.");
            return Results.NoContent();
        }
    }
}
