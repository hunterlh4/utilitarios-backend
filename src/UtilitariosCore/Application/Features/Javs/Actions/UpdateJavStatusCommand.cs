using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record UpdateJavStatusCommand(int Id, ContentStatus Status) : IRequest<Result>
{
    internal sealed class Handler(IJavRepository javRepository) 
        : IRequestHandler<UpdateJavStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateJavStatusCommand request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            await javRepository.UpdateJavStatus(request.Id, request.Status);

            return Results.NoContent();
        }
    }
}
