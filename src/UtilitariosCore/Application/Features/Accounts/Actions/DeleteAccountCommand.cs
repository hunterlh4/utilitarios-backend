using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record DeleteAccountCommand(int Id) : IRequest<Result>;

internal sealed class DeleteAccountCommandHandler(IAccountRepository accountRepository)
    : IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await accountRepository.Delete(request.Id);
        if (!deleted) return Errors.NotFound("Cuenta no encontrada.");
        return Results.NoContent();
    }
}
