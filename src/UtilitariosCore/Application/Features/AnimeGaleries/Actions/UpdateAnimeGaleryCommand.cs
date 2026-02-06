using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record UpdateAnimeGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IAnimeGaleryRepository repository) 
        : IRequestHandler<UpdateAnimeGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateAnimeGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetAnimeGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            item.Name = request.Name;
            await repository.UpdateAnimeGalery(item);

            return Results.NoContent();
        }
    }
}
