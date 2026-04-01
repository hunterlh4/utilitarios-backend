using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetAllGitHubAccountsQuery : IRequest<Result<IEnumerable<AccountGitHubDto>>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetAllGitHubAccountsQuery, Result<IEnumerable<AccountGitHubDto>>>
    {
        public async Task<Result<IEnumerable<AccountGitHubDto>>> Handle(GetAllGitHubAccountsQuery r, CancellationToken ct)
        {
            var result = await repo.GetGitHubs();
            return result.ToList();
        }
    }
}
