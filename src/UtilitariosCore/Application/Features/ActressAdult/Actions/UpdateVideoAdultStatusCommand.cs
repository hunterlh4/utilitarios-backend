using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record UpdateVideoAdultStatusCommand(int VideoId, char Status) : IRequest<Result>;

internal sealed class UpdateVideoAdultStatusCommandHandler(IVideoAdultRepository videoAdultRepository) 
    : IRequestHandler<UpdateVideoAdultStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateVideoAdultStatusCommand request, CancellationToken cancellationToken)
    {
        var video = await videoAdultRepository.GetVideoAdultById(request.VideoId);
        
        if (video == null)
        {
            return Errors.NotFound("Video not found.");
        }

        if (request.Status != '0' && request.Status != '1')
        {
            return Errors.BadRequest("Status must be '0' (proximamente) or '1' (completado).");
        }

        video.Status = request.Status;
        await videoAdultRepository.UpdateVideoAdult(video);

        return Results.NoContent();
    }
}
