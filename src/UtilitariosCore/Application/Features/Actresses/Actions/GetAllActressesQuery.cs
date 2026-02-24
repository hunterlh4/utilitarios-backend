using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record GetAllActressesQuery : IRequest<Result<IEnumerable<ActressJavDto>>>;

internal sealed class GetAllActressesQueryHandler(IActressRepository actressRepository)
    : IRequestHandler<GetAllActressesQuery, Result<IEnumerable<ActressJavDto>>>
{
    public async Task<Result<IEnumerable<ActressJavDto>>> Handle(GetAllActressesQuery request, CancellationToken cancellationToken)
    {
        var result = await actressRepository.GetAllActressesWithFirstImage();
        return result.ToList();
    }
}
