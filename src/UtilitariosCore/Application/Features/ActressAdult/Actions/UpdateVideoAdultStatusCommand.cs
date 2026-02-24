using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record UpdateVideoAdultStatusCommand : IRequest<Result>
{
    public int VideoId { get; set; }
    public ContentStatus Status { get; set; }

    public sealed class Validator : AbstractValidator<UpdateVideoAdultStatusCommand>
    {
        public Validator()
        {
            RuleFor(x => x.VideoId).GreaterThan(0).WithMessage("El ID del video debe ser mayor a 0.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("El estado no es válido.");
        }
    }

    internal sealed class Handler(IVideoAdultRepository videoAdultRepository)
        : IRequestHandler<UpdateVideoAdultStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateVideoAdultStatusCommand request, CancellationToken cancellationToken)
        {
            var video = await videoAdultRepository.GetVideoAdultById(request.VideoId);
            if (video == null) return Errors.NotFound("Video no encontrado.");

            video.Status = request.Status;
            await videoAdultRepository.UpdateVideoAdult(video);

            return Results.NoContent();
        }
    }
}
