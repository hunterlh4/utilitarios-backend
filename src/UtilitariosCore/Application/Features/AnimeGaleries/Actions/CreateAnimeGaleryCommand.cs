using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.AnimeGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public class CreateAnimeGaleryCommand : IRequest<Result<CreateAnimeGaleryDto>>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<CreateAnimeGaleryCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(IAnimeGaleryRepository repository) 
        : IRequestHandler<CreateAnimeGaleryCommand, Result<CreateAnimeGaleryDto>>
    {
        public async Task<Result<CreateAnimeGaleryDto>> Handle(CreateAnimeGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = new AnimeGalery
            {
                Name = StringNormalizer.ToTitleCase(request.Name),
                CreatedAt = DateTime.UtcNow
            };

            var id = await repository.CreateAnimeGalery(item);

            return Results.Created(new CreateAnimeGaleryDto { Id = id });
        }
    }
}
