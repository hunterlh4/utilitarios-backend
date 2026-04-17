using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record UpdateHentaiStatusCommand : IRequest<Result>
{
    public int Id { get; set; }
    public ContentStatus Status { get; set; }

    public sealed class Validator : AbstractValidator<UpdateHentaiStatusCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("El estado no es válido.");
        }
    }

    internal sealed class Handler(IHentaiRepository hentaiRepository)
        : IRequestHandler<UpdateHentaiStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateHentaiStatusCommand request, CancellationToken cancellationToken)
        {
            var item = await hentaiRepository.GetHentaiById(request.Id);
            if (item is null) return Errors.NotFound("Hentai no encontrado.");

            item.Status = request.Status;
            await hentaiRepository.UpdateHentai(item);

            return Results.NoContent();
        }
    }
}