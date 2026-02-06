using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Series.Actions;

public record DeleteSeriesCommand(int Id) : IRequest<Result>;

internal sealed class DeleteSeriesCommandHandler(ISeriesRepository seriesRepository) 
    : IRequestHandler<DeleteSeriesCommand, Result>
{
    public async Task<Result> Handle(DeleteSeriesCommand request, CancellationToken cancellationToken)
    {
        var item = await seriesRepository.GetSeriesById(request.Id);

        if (item is null)
        {
            return Errors.NotFound();
        }

        await seriesRepository.DeleteSeries(request.Id);

        return Results.NoContent();
    }
}
