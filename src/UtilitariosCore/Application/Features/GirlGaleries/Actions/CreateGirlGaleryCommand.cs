using MediatR;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public class CreateGirlGaleryCommand : IRequest<Result<CreateGirlGaleryDto>>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IGirlGaleryRepository repository) 
        : IRequestHandler<CreateGirlGaleryCommand, Result<CreateGirlGaleryDto>>
    {
        public async Task<Result<CreateGirlGaleryDto>> Handle(CreateGirlGaleryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var item = new GirlGalery
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var id = await repository.CreateGirlGalery(item);

            return Results.Created(new CreateGirlGaleryDto { Id = id });
        }
    }
}
