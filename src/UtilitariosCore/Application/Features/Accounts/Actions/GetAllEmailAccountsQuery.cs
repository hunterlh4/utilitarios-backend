using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAllEmailAccountsQuery : IRequest<Result<IEnumerable<AccountEmailDto>>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetAllEmailAccountsQuery, Result<IEnumerable<AccountEmailDto>>>
    {
        public async Task<Result<IEnumerable<AccountEmailDto>>> Handle(GetAllEmailAccountsQuery r, CancellationToken ct)
        {
            var result = await repo.GetEmails();
            return result.ToList();
        }
    }
}
