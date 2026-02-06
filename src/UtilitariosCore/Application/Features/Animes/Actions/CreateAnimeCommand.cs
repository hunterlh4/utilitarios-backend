using MediatR;
using UtilitariosCore.Application.Features.Animes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public class CreateAnimeCommand : IRequest<Result<CreateAnimeDto>>
{
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }

    internal sealed class Handler(IAnimeRepository animeRepository) 
        : IRequestHandler<CreateAnimeCommand, Result<CreateAnimeDto>>
    {
        public async Task<Result<CreateAnimeDto>> Handle(CreateAnimeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ApiId))
            {
                return Errors.BadRequest("ApiId is required.");
            }

            var existingItem = await animeRepository.GetAnimeByApiId(request.ApiId);

            if (existingItem != null)
            {
                return Errors.BadRequest($"Anime with ApiId {request.ApiId} already exists.");
            }

            var newItem = new Anime
            {
                ApiId = request.ApiId,
                Title = request.Title,
                Image = request.Image,
                Episodes = request.Episodes,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            var itemId = await animeRepository.CreateAnime(newItem);

            return Results.Created(new CreateAnimeDto
            {
                Id = itemId
            });
        }
    }
}
