using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record UpdateVideoAdultCommand : IRequest<Result>
{
    public int Id { get; set; }
    public List<int> ActressIds { get; set; } = [];
    public List<int> TagIds { get; set; } = [];

    public sealed class Validator : AbstractValidator<UpdateVideoAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(
        IVideoAdultRepository videoAdultRepository,
        ITagRepository tagRepository)
        : IRequestHandler<UpdateVideoAdultCommand, Result>
    {
        public async Task<Result> Handle(UpdateVideoAdultCommand request, CancellationToken cancellationToken)
        {
            var video = await videoAdultRepository.GetVideoAdultById(request.Id);
            if (video is null) return Errors.NotFound("Video no encontrado.");

            // Diff actrices
            var currentActressIds = (await videoAdultRepository.GetActressIdsByVideoId(request.Id)).ToHashSet();
            var incomingActressIds = request.ActressIds.ToHashSet();

            foreach (var id in currentActressIds.Except(incomingActressIds))
                await videoAdultRepository.RemoveActressFromVideo(request.Id, id);

            foreach (var id in incomingActressIds.Except(currentActressIds))
                await videoAdultRepository.AddActressToVideo(request.Id, id);

            // Reemplazar tags
            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.VideoAdult, request.TagIds);

            return Results.NoContent();
        }
    }
}
