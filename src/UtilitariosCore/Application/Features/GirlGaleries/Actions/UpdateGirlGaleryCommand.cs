using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record UpdateGirlGaleryCommand(int Id) : IRequest<Result>
{
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IGirlGaleryRepository repository) 
        : IRequestHandler<UpdateGirlGaleryCommand, Result>
    {
        public async Task<Result> Handle(UpdateGirlGaleryCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetGirlGaleryById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            item.Name = request.Name;
            await repository.UpdateGirlGalery(item);

            return Results.NoContent();
        }
    }
}
