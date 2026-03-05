using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAccountByIdQuery(int Id) : IRequest<Result<AccountDto>>;

internal sealed class GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    : IRequestHandler<GetAccountByIdQuery, Result<AccountDto>>
{
    public async Task<Result<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetById(request.Id);
        if (account is null) return Errors.NotFound("Cuenta no encontrada.");
        return account;
    }
}
