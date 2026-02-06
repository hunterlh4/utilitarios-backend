using MediatR;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public class CreateAnimeGaleryCommand : IRequest<Result<CreateAnimeGaleryDto>>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IAnimeGaleryRepository repository) 
        : IRequestHandler<CreateAnimeGaleryCommand, Result<CreateAnimeGaleryDto>>
    {
        public async Task<Result<CreateAnimeGaleryDto>> Handle(CreateAnimeGaleryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var item = new AnimeGalery
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var id = await repository.CreateAnimeGalery(item);

            return Results.Created(new CreateAnimeGaleryDto { Id = id });
        }
    }
}
