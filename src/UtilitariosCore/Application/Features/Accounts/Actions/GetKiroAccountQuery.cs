using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetKiroAccountQuery : IRequest<Result<IEnumerable<AccountKiroDto>>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetKiroAccountQuery, Result<IEnumerable<AccountKiroDto>>>
    {
        public async Task<Result<IEnumerable<AccountKiroDto>>> Handle(GetKiroAccountQuery request, CancellationToken cancellationToken)
        {
            var result = await repo.GetKiro();
            return result.ToList() ;
        }
    }
}
