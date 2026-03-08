using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record DeleteVideoAdultCommand(int VideoId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteVideoAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.VideoId).GreaterThan(0).WithMessage("El ID del video debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(IVideoAdultRepository videoAdultRepository)
        : IRequestHandler<DeleteVideoAdultCommand, Result>
    {
        public async Task<Result> Handle(DeleteVideoAdultCommand request, CancellationToken cancellationToken)
        {
            var video = await videoAdultRepository.GetVideoAdultById(request.VideoId);
            if (video is null) return Errors.NotFound("Video no encontrado.");

            // El repositorio debe manejar la eliminación en cascada de relaciones
            var deleted = await videoAdultRepository.DeleteVideoAdult(request.VideoId);
            
            return deleted ? Results.NoContent() : Errors.BadRequest("No se pudo eliminar el video.");
        }
    }
}
