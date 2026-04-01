using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record GetKiroAccountQuery : IRequest<Result<AccountKiroDto?>>
{
    internal sealed class Handler(IAccountRepository repo)
        : IRequestHandler<GetKiroAccountQuery, Result<AccountKiroDto?>>
    {
        public async Task<Result<AccountKiroDto?>> Handle(GetKiroAccountQuery r, CancellationToken ct)
        {
            var result = await repo.GetKiro();
            return result;
        }
    }
}
