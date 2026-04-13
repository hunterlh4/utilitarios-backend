using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record UpdateGirlGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<UpdateGirlGaleryCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(
        IGaleryRepository repository) 
        : IRequestHandler<UpdateGirlGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateGirlGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetGirlGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            item.Name = StringNormalizer.ToTitleCase(request.Name);
            await repository.UpdateGirlGalery(item);

            return Results.NoContent();
        }
    }
}
