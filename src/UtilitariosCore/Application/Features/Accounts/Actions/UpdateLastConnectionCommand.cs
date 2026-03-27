using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateLastConnectionCommand(int Id) : IRequest<Result>;

internal sealed class UpdateLastConnectionCommandHandler(IAccountRepository accountRepository)
    : IRequestHandler<UpdateLastConnectionCommand, Result>
{
    public async Task<Result> Handle(UpdateLastConnectionCommand request, CancellationToken cancellationToken)
    {
        if (!await accountRepository.Exists(request.Id))
            return Errors.NotFound("Cuenta no encontrada.");

        await accountRepository.UpdateLastConnection(request.Id, DateTime.Now);
        return Results.NoContent();
    }
}
