using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record UpdateSeriesStatusCommand(int Id, ContentStatus Status) : IRequest<Result>;

internal sealed class UpdateSeriesStatusCommandHandler(ISeriesRepository seriesRepository) 
    : IRequestHandler<UpdateSeriesStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateSeriesStatusCommand request, CancellationToken cancellationToken)
    {
        var item = await seriesRepository.GetSeriesById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        item.Status = request.Status;
        await seriesRepository.UpdateSeries(item);

        return Results.NoContent();
    }
}
