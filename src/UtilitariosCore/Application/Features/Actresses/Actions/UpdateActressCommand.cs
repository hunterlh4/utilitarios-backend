using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record UpdateActressCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<UpdateActressCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(IActressJavRepository actressRepository)
        : IRequestHandler<UpdateActressCommand, Result>
    {
        public async Task<Result> Handle(UpdateActressCommand request, CancellationToken cancellationToken)
        {
            var actress = await actressRepository.GetActressById(request.Id);
            if (actress == null) return Errors.NotFound("Actriz no encontrada.");

            actress.Name = request.Name;
            await actressRepository.UpdateActress(actress);

            return Results.NoContent();
        }
    }
}
