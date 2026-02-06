using MediatR;
using UtilitariosCore.Application.Features.Hentais.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public class CreateHentaiCommand : IRequest<Result<CreateHentaiDto>>
{
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public ContentStatus Status { get; set; }

    internal sealed class Handler(IHentaiRepository hentaiRepository) 
        : IRequestHandler<CreateHentaiCommand, Result<CreateHentaiDto>>
    {
        public async Task<Result<CreateHentaiDto>> Handle(CreateHentaiCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ApiId))
            {
                return Errors.BadRequest("ApiId is required.");
            }

            var existingItem = await hentaiRepository.GetHentaiByApiId(request.ApiId);

            if (existingItem != null)
            {
                return Errors.BadRequest($"Hentai with ApiId {request.ApiId} already exists.");
            }

            var newItem = new Hentai
            {
                ApiId = request.ApiId,
                Title = request.Title,
                Image = request.Image,
                Episodes = request.Episodes,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            var itemId = await hentaiRepository.CreateHentai(newItem);

            return Results.Created(new CreateHentaiDto
            {
                Id = itemId
            });
        }
    }
}
