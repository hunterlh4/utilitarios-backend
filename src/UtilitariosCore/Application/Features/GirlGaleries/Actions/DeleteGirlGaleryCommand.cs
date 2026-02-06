using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record DeleteGirlGaleryCommand(int Id) : IRequest<Result>;

internal sealed class DeleteGirlGaleryCommandHandler(IGirlGaleryRepository repository) 
    : IRequestHandler<DeleteGirlGaleryCommand, Result>
{
    public async Task<Result> Handle(DeleteGirlGaleryCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetGirlGaleryById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        await repository.DeleteGirlGalery(request.Id);

        return Results.NoContent();
    }
}
