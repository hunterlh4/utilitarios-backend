using FluentValidation;
using MediatR;
using System.Text.RegularExpressions;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Application.Features.Metadata;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record CreateVideoAdultCommand : IRequest<Result<CreateVideoAdultDto>>
{
    public VideoAdultSource Source { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = [];

    public sealed class Validator : AbstractValidator<CreateVideoAdultCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Source).IsInEnum().WithMessage("La fuente no es válida.");
            RuleFor(x => x.VideoUrl).NotEmpty().WithMessage("La URL del video es requerida.");
        }
    }

    internal sealed class Handler(
        IVideoAdultRepository videoAdultRepository,
        ISender sender)
        : IRequestHandler<CreateVideoAdultCommand, Result<CreateVideoAdultDto>>
    {
        public async Task<Result<CreateVideoAdultDto>> Handle(CreateVideoAdultCommand request, CancellationToken cancellationToken)
        {
            var sourceName = request.Source.ToString().ToLower();
            var externalId = ExtractExternalId(request.VideoUrl, request.Source);
            if (string.IsNullOrWhiteSpace(externalId)) return Errors.BadRequest("No se pudo extraer el ID del video desde la URL.");

            var metadataResult = await sender.Send(new GetMetadataQuery(request.VideoUrl), cancellationToken);

            string? title = null;
            string? thumbnailUrl = null;

            if (metadataResult.IsSuccess && metadataResult.Value != null)
            {
                title = metadataResult.Value.Data?.Title;
                thumbnailUrl = metadataResult.Value.Data?.Image?.Url;
            }

            var newVideo = new VideoAdult
            {
                Source = sourceName,
                ExternalId = externalId,
                VideoUrl = request.VideoUrl,
                Title = title,
                ThumbnailUrl = thumbnailUrl,
                Status = ContentStatus.Proximamente,
                CreatedAt = DateTime.UtcNow
            };

            var videoId = await videoAdultRepository.CreateVideoAdult(newVideo);

            if (request.ActressIds.Count != 0)
            {
                foreach (var actressId in request.ActressIds)
                    await videoAdultRepository.AddActressToVideo(videoId, actressId);
            }

            return Results.Created(new CreateVideoAdultDto { Id = videoId });
        }

        private static string ExtractExternalId(string url, VideoAdultSource source)
        {
            try
            {
                if (source == VideoAdultSource.Pornhub)
                {
                    var match = Regex.Match(url, @"viewkey=([^&]+)");
                    return match.Success ? match.Groups[1].Value : string.Empty;
                }

                if (source == VideoAdultSource.Xvideos)
                {
                    var match = Regex.Match(url, @"/video(\d+)/");
                    return match.Success ? match.Groups[1].Value : string.Empty;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
