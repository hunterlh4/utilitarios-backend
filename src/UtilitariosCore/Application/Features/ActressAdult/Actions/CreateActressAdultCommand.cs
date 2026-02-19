using MediatR;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public class CreateActressAdultCommand : IRequest<Result<CreateActressAdultDto>>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IActressAdultRepository actressAdultRepository) 
        : IRequestHandler<CreateActressAdultCommand, Result<CreateActressAdultDto>>
    {
        public async Task<Result<CreateActressAdultDto>> Handle(CreateActressAdultCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var newActress = new ActressAdult
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var actressId = await actressAdultRepository.CreateActressAdult(newActress);

            return Results.Created(new CreateActressAdultDto
            {
                Id = actressId
            });
        }
    }
}
