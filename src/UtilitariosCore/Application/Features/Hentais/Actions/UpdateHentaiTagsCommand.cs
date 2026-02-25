using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record UpdateHentaiTagsCommand(int Id, List<int> TagIds) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateHentaiTagsCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(
        IHentaiRepository hentaiRepository,
        ITagRepository tagRepository)
        : IRequestHandler<UpdateHentaiTagsCommand, Result>
    {
        public async Task<Result> Handle(UpdateHentaiTagsCommand request, CancellationToken cancellationToken)
        {
            var hentai = await hentaiRepository.GetHentaiById(request.Id);
            if (hentai is null) return Errors.NotFound("Hentai no encontrado.");

            await tagRepository.ReplaceTagsForRefId(request.Id, TagType.Hentai, request.TagIds);

            return Results.NoContent();
        }
    }
}
