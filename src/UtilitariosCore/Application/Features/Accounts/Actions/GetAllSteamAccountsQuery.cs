using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAllSteamAccountsQuery : IRequest<Result<IEnumerable<AccountSteamDto>>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetAllSteamAccountsQuery, Result<IEnumerable<AccountSteamDto>>>
    {
        public async Task<Result<IEnumerable<AccountSteamDto>>> Handle(GetAllSteamAccountsQuery r, CancellationToken ct)
        {
            var result = await repo.GetSteams();
            return result.ToList();
        }
    }
}
