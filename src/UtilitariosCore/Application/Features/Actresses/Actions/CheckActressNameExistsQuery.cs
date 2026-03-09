using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record CheckActressNameExistsQuery(string Name) : IRequest<Result<bool>>;

internal sealed class CheckActressNameExistsQueryHandler(IActressJavRepository actressRepository)
    : IRequestHandler<CheckActressNameExistsQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckActressNameExistsQuery request, CancellationToken cancellationToken)
    {
        var exists = await actressRepository.CheckActressNameExists(request.Name);
        return exists;
    }
}
