using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.YouTubes.Actions;

public record DeleteYouTubeCommand(int Id) : IRequest<Result<bool>>;

internal sealed class DeleteYouTubeCommandHandler(IYouTubeRepository youTubeRepository)
    : IRequestHandler<DeleteYouTubeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteYouTubeCommand request, CancellationToken cancellationToken)
    {
        var success = await youTubeRepository.Delete(request.Id);
        if (!success) return Errors.NotFound("El video no existe.");
        return true;
    }
}
