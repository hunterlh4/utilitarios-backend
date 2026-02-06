using MediatR;
using UtilitariosCore.Application.Features.Series.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Series.Actions;

public class CreateSeriesCommand : IRequest<Result<CreateSeriesDto>>
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }

    internal sealed class Handler(ISeriesRepository seriesRepository) 
        : IRequestHandler<CreateSeriesCommand, Result<CreateSeriesDto>>
    {
        public async Task<Result<CreateSeriesDto>> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ImdbId))
            {
                return Errors.BadRequest("ImdbId is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return Errors.BadRequest("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Image))
            {
                return Errors.BadRequest("Image is required.");
            }

            var newSeries = new Domain.Models.Series
            {
                ImdbId = request.ImdbId,
                Title = request.Title,
                Image = request.Image,
                Year = request.Year,
                Rating = request.Rating,
                Type = request.Type,
                Status = ContentStatus.Proximamente,
                CreatedAt = DateTime.UtcNow
            };

            var id = await seriesRepository.CreateSeries(newSeries);

            return Results.Created(new CreateSeriesDto { Id = id });
        }
    }
}
