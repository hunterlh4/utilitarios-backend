using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Media.Actions;

public record DeleteMediaCommand(int Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteMediaCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(IMediaRepository mediaRepository)
        : IRequestHandler<DeleteMediaCommand, Result>
    {
        public async Task<Result> Handle(DeleteMediaCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetMediaById(request.Id);
            if (media == null) return Errors.NotFound("Media no encontrada.");

           //await imgBBService.DeleteImageAsync(media.DeleteUrl);

            await mediaRepository.DeleteMedia(request.Id);

            return Results.NoContent();
        }
    }
}
