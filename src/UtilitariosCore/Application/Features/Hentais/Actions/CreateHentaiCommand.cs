using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Hentais.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public class CreateHentaiCommand : IRequest<Result<CreateHentaiDto>>
{
    public string ApiId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }

    public sealed class Validator : AbstractValidator<CreateHentaiCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ApiId).NotEmpty().WithMessage("El ApiId es requerido.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("El título es requerido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.Episodes).GreaterThan(0).WithMessage("Los episodios deben ser mayor a 0.");
        }
    }

    internal sealed class Handler(IHentaiRepository hentaiRepository) 
        : IRequestHandler<CreateHentaiCommand, Result<CreateHentaiDto>>
    {
        public async Task<Result<CreateHentaiDto>> Handle(CreateHentaiCommand request, CancellationToken cancellationToken)
        {
            var existingItem = await hentaiRepository.GetHentaiByApiId(request.ApiId);

            if (existingItem != null)
            {
                return Errors.BadRequest($"Hentai with ApiId {request.ApiId} already exists.");
            }

            var newItem = new Hentai
            {
                ApiId = request.ApiId,
                Title = StringNormalizer.ToTitleCase(request.Title),
                Image = request.Image,
                Episodes = request.Episodes,
                Status = ContentStatus.Proximamente,
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
