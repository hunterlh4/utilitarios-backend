using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record GetAllActressAdultsQuery : IRequest<Result<IEnumerable<ActressAdultDto>>>;

internal sealed class GetAllActressAdultsQueryHandler(IActressAdultRepository actressAdultRepository)
    : IRequestHandler<GetAllActressAdultsQuery, Result<IEnumerable<ActressAdultDto>>>
{
    public async Task<Result<IEnumerable<ActressAdultDto>>> Handle(GetAllActressAdultsQuery request, CancellationToken cancellationToken)
    {
        var result = await actressAdultRepository.GetAllActressAdultsWithFirstImage();
        return result.ToList();
    }
}
