using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAllGeneralAccountsQuery : IRequest<Result<IEnumerable<AccountGeneralDto>>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetAllGeneralAccountsQuery, Result<IEnumerable<AccountGeneralDto>>>
    {
        public async Task<Result<IEnumerable<AccountGeneralDto>>> Handle(GetAllGeneralAccountsQuery r, CancellationToken ct)
        {
            var result = await repo.GetGenerals();
            return result.ToList();
        }
    }
}
