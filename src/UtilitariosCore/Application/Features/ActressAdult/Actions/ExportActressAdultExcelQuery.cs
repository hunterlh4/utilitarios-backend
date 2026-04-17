using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record ExportActressAdultExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportActressAdultExcelQueryHandler(
    IActressAdultRepository repository,
    IVideoAdultRepository videoAdultRepository,
    ITagRepository tagRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<ExportActressAdultExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressAdultExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressAdults()).ToList();
        var actressRows = new List<ActressAdultExcelRow>();
        var actressLinkRows = new List<ActressAdultLinkExcelRow>();

        foreach (var actress in actresses)
        {
            var actressTagIds = (await tagRepository.GetTagsByRefId(actress.Id, TagType.ActressAdult))
                .Select(t => t.Id)
                .ToList();
            actressRows.Add(new ActressAdultExcelRow
            {
                Id = actress.Id,
                Name = actress.Name,
                Image = actress.Image,
                TagIds = actressTagIds.Any() ? string.Join(',', actressTagIds.OrderBy(x => x)) : null,
            });

            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressAdult);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
            {
                actressLinkRows.Add(new ActressAdultLinkExcelRow
                {
                    ActressAdultId = actress.Id,
                    ActressAdultName = actress.Name,
                    Url = link.Url,
                    OrderIndex = link.OrderIndex ?? 0,
                });
            }
        }

        var videos = (await videoAdultRepository.GetAllVideoAdults()).ToList();
        var videoRows = new List<VideoAdultExcelRow>();
        var videoLinkRows = new List<VideoAdultLinkExcelRow>();

        foreach (var video in videos)
        {
            var videoTagIds = (await tagRepository.GetTagsByRefId(video.Id, TagType.VideoAdult))
                .Select(t => t.Id)
                .ToList();
            var actressIds = (await videoAdultRepository.GetActressIdsByVideoId(video.Id)).ToList();
            var actressNames = actresses
                .Where(a => actressIds.Contains(a.Id))
                .Select(a => a.Name)
                .ToList();

            videoRows.Add(new VideoAdultExcelRow
            {
                Id = video.Id,
                Source = video.Source,
                ExternalId = video.ExternalId,
                VideoUrl = video.VideoUrl,
                Title = video.Title,
                ThumbnailUrl = video.ThumbnailUrl,
                Status = (int)video.Status,
                TagIds = videoTagIds.Any() ? string.Join(',', videoTagIds.OrderBy(x => x)) : null,
                ActressIds = actressIds.Any() ? string.Join(',', actressIds.OrderBy(x => x)) : null,
                ActressNames = actressNames.Any() ? string.Join(", ", actressNames) : null,
            });

            if (!string.IsNullOrWhiteSpace(video.VideoUrl))
            {
                videoLinkRows.Add(new VideoAdultLinkExcelRow
                {
                    VideoAdultId = video.Id,
                    VideoExternalId = video.ExternalId,
                    Url = video.VideoUrl,
                    OrderIndex = 1,
                });
            }
        }

        using var stream = ExcelHelper.CreateActressAdultExcel(actressRows, actressLinkRows, videoRows, videoLinkRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"actress-adult-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
