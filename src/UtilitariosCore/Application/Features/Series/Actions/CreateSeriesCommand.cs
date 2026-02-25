using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Series.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Series.Actions;

public class CreateSeriesCommand : IRequest<Result<CreateSeriesDto>>
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }

    public sealed class Validator : AbstractValidator<CreateSeriesCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ImdbId).NotEmpty().WithMessage("El ImdbId es requerido.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("El título es requerido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.Rating).InclusiveBetween(0, 10).When(x => x.Rating.HasValue)
                .WithMessage("El rating debe estar entre 0 y 10.");
        }
    }

    internal sealed class Handler(ISeriesRepository seriesRepository) 
        : IRequestHandler<CreateSeriesCommand, Result<CreateSeriesDto>>
    {
        public async Task<Result<CreateSeriesDto>> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
        {
            var newSeries = new Domain.Models.Series
            {
                ImdbId = request.ImdbId,
                Title = StringNormalizer.ToTitleCase(request.Title),
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
