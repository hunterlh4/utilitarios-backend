using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record DeleteAnimeGaleryCommand(int Id) : IRequest<Result>;

internal sealed class DeleteAnimeGaleryCommandHandler(IAnimeGaleryRepository repository) 
    : IRequestHandler<DeleteAnimeGaleryCommand, Result>
{
    public async Task<Result> Handle(DeleteAnimeGaleryCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetAnimeGaleryById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        await repository.DeleteAnimeGalery(request.Id);

        return Results.NoContent();
    }
}
