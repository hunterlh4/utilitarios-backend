using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record CreateActressAdultCommand : IRequest<Result<CreateActressAdultDto>>
{
    public string Name { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<CreateActressAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerida.");
        }
    }

    internal sealed class Handler(
        IActressAdultRepository actressAdultRepository)
        : IRequestHandler<CreateActressAdultCommand, Result<CreateActressAdultDto>>
    {
        public async Task<Result<CreateActressAdultDto>> Handle(CreateActressAdultCommand request, CancellationToken cancellationToken)
        {
            var newActress = new ActressAdult
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var actressId = await actressAdultRepository.CreateActressAdult(newActress);

            return Results.Created(new CreateActressAdultDto { Id = actressId });
        }
    }
}
