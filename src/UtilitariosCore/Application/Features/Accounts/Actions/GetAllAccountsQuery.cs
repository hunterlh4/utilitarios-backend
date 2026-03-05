using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAllAccountsQuery(AccountType? Type = null) : IRequest<Result<IEnumerable<AccountDto>>>;

internal sealed class GetAllAccountsQueryHandler(IAccountRepository accountRepository)
    : IRequestHandler<GetAllAccountsQuery, Result<IEnumerable<AccountDto>>>
{
    public async Task<Result<IEnumerable<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var result = await accountRepository.GetAll(request.Type);
        return result.ToList();
    }
}
