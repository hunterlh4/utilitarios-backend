using MediatR;
using UtilitariosCore.Application.Features.Metadata;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using System.Text.RegularExpressions;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public class CreateVideoAdultCommand : IRequest<Result<CreateVideoAdultDto>>
{
    public string Source { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = new();

    internal sealed class Handler(
        IVideoAdultRepository videoAdultRepository,
        ISender sender) 
        : IRequestHandler<CreateVideoAdultCommand, Result<CreateVideoAdultDto>>
    {
        public async Task<Result<CreateVideoAdultDto>> Handle(CreateVideoAdultCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Source))
            {
                return Errors.BadRequest("Source is required.");
            }

            if (string.IsNullOrWhiteSpace(request.VideoUrl))
            {
                return Errors.BadRequest("VideoUrl is required.");
            }

            // Extraer external_id de la URL
            string externalId = ExtractExternalId(request.VideoUrl, request.Source);
            
            if (string.IsNullOrWhiteSpace(externalId))
            {
                return Errors.BadRequest("Could not extract video ID from URL.");
            }

            // Obtener metadata del video
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
                Source = request.Source,
                ExternalId = externalId,
                VideoUrl = request.VideoUrl,
                Title = title,
                ThumbnailUrl = thumbnailUrl,
                Status = '0',
                CreatedAt = DateTime.UtcNow
            };

            var videoId = await videoAdultRepository.CreateVideoAdult(newVideo);

            // Asociar actrices al video
            if (request.ActressIds != null && request.ActressIds.Any())
            {
                foreach (var actressId in request.ActressIds)
                {
                    await videoAdultRepository.AddActressToVideo(videoId, actressId);
                }
            }

            return Results.Created(new CreateVideoAdultDto
            {
                Id = videoId
            });
        }

        private static string ExtractExternalId(string url, string source)
        {
            try
            {
                if (source.ToLower() == "pornhub")
                {
                    // Extraer viewkey de pornhub
                    var match = Regex.Match(url, @"viewkey=([^&]+)");
                    return match.Success ? match.Groups[1].Value : string.Empty;
                }
                else if (source.ToLower() == "xvideos")
                {
                    // Extraer ID de xvideos (ej: /video12345/title)
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
