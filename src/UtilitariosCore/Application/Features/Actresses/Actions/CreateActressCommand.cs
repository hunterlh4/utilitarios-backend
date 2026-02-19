using MediatR;
using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public class CreateActressCommand : IRequest<Result<CreateActressJavDto>>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IActressRepository actressRepository) 
        : IRequestHandler<CreateActressCommand, Result<CreateActressJavDto>>
    {
        public async Task<Result<CreateActressJavDto>> Handle(CreateActressCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var newActress = new Actress
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var actressId = await actressRepository.CreateActress(newActress);

            return Results.Created(new CreateActressJavDto
            {
                Id = actressId
            });
        }
    }
}
