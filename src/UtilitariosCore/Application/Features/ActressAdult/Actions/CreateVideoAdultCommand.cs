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
    public List<int> TagIds { get; set; } = [];

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
        ITagRepository tagRepository,
        ISender sender)
        : IRequestHandler<CreateVideoAdultCommand, Result<CreateVideoAdultDto>>
    {
        public async Task<Result<CreateVideoAdultDto>> Handle(CreateVideoAdultCommand request, CancellationToken cancellationToken)
        {
            var sourceName = request.Source.ToString().ToLower();
            var externalId = ExtractExternalId(request.VideoUrl, request.Source);
            if (string.IsNullOrWhiteSpace(externalId)) return Errors.BadRequest("No se pudo extraer el ID del video desde la URL.");

            // Verificar si el video ya existe
            var existingVideo = await videoAdultRepository.GetVideoAdultBySourceAndExternalId(sourceName, externalId);
            if (existingVideo != null)
            {
                return Errors.BadRequest($"Este video ya existe en la base de datos (ID: {existingVideo.Id})");
            }

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
                Status = ContentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var videoId = await videoAdultRepository.CreateVideoAdult(newVideo);

            if (request.ActressIds.Count != 0)
            {
                foreach (var actressId in request.ActressIds)
                    await videoAdultRepository.AddActressToVideo(videoId, actressId);
            }

            if (request.TagIds.Count > 0)
                await tagRepository.ReplaceTagsForRefId(videoId, TagType.VideoAdult, request.TagIds);

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
                    // Xvideos usa formato: /video.XXXXXX/ o /video/XXXXXX/
                    var match = Regex.Match(url, @"/video[./]([a-zA-Z0-9]+)");
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
