using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public class CreateGirlGaleryCommand : IRequest<Result<CreateGirlGaleryDto>>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<CreateGirlGaleryCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(IGirlGaleryRepository repository) 
        : IRequestHandler<CreateGirlGaleryCommand, Result<CreateGirlGaleryDto>>
    {
        public async Task<Result<CreateGirlGaleryDto>> Handle(CreateGirlGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = new GirlGalery
            {
                Name = StringNormalizer.ToTitleCase(request.Name),
                CreatedAt = DateTime.UtcNow
            };

            var id = await repository.CreateGirlGalery(item);

            return Results.Created(new CreateGirlGaleryDto { Id = id });
        }
    }
}
