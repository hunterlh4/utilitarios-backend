using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record DeleteJavCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IJavRepository javRepository) 
        : IRequestHandler<DeleteJavCommand, Result>
    {
        public async Task<Result> Handle(DeleteJavCommand request, CancellationToken cancellationToken)
        {
            var item = await javRepository.GetJavById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            await javRepository.DeleteJav(request.Id);
            return Results.NoContent();
        }
    }
}
