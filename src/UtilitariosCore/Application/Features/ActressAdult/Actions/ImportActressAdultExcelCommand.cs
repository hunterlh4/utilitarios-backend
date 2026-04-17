using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record ImportActressAdultExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportActressAdultExcelCommandHandler(
    IActressAdultRepository repository,
    IVideoAdultRepository videoAdultRepository,
    ITagRepository tagRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<ImportActressAdultExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportActressAdultExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var data = ExcelHelper.ReadActressAdultExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var actressByExcelId = new Dictionary<int, ActressAdult>();
        var actressByName = new Dictionary<string, ActressAdult>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in data.Actresses)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(row.Name);
            var existing = await repository.GetActressAdultByName(normalizedName);
            var parsedTagIds = ParseIds(row.TagIds);

            if (existing is null)
            {
                var createdId = await repository.CreateActressAdult(new ActressAdult
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                });

                var createdActress = await repository.GetActressAdultById(createdId);
                if (createdActress is not null)
                {
                    if (parsedTagIds.Count > 0)
                        await tagRepository.ReplaceTagsForRefId(createdActress.Id, TagType.ActressAdult, parsedTagIds);

                    actressByName[createdActress.Name] = createdActress;
                    if (row.Id > 0)
                        actressByExcelId[row.Id] = createdActress;
                }

                created++;
                continue;
            }

            var nextImage = string.IsNullOrWhiteSpace(row.Image) ? existing.Image : row.Image;
            var hasChanges = existing.Name != normalizedName || existing.Image != nextImage;

            if (!hasChanges)
            {
                skipped++;
            }
            else
            {
                existing.Name = normalizedName;
                existing.Image = nextImage;
                await repository.UpdateActressAdult(existing);
                updated++;
            }

            if (parsedTagIds.Count > 0)
                await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.ActressAdult, parsedTagIds);

            actressByName[existing.Name] = existing;
            if (row.Id > 0)
                actressByExcelId[row.Id] = existing;
        }

        var actressLinksByActressId = data.ActressLinks
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .GroupBy(x => x.ActressAdultId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.OrderIndex <= 0 ? int.MaxValue : x.OrderIndex)
                      .Select(x => x.Url.Trim())
                      .Where(url => !string.IsNullOrWhiteSpace(url))
                      .Distinct(StringComparer.OrdinalIgnoreCase)
                      .ToList());

        foreach (var actressRow in data.Actresses)
        {
            ActressAdult? actress = null;
            if (actressRow.Id > 0)
                actressByExcelId.TryGetValue(actressRow.Id, out actress);

            if (actress is null)
                actressByName.TryGetValue(StringNormalizer.ToTitleCaseWithNumbers(actressRow.Name), out actress);

            if (actress is null)
                continue;

            if (!actressLinksByActressId.TryGetValue(actressRow.Id, out var urls))
                continue;

            await SyncActressLinks(actress.Id, urls);
        }

        var videoByExcelId = new Dictionary<int, VideoAdult>();
        var videoByExternalId = new Dictionary<string, VideoAdult>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in data.Videos)
        {
            if (string.IsNullOrWhiteSpace(row.Source) || string.IsNullOrWhiteSpace(row.ExternalId))
            {
                invalid++;
                continue;
            }

            var normalizedSource = row.Source.Trim().ToLowerInvariant();
            var existingVideo = await videoAdultRepository.GetVideoAdultBySourceAndExternalId(normalizedSource, row.ExternalId.Trim());
            var status = Enum.IsDefined(typeof(ContentStatus), row.Status)
                ? (ContentStatus)row.Status
                : ContentStatus.Pending;
            var videoUrlFromLinks = ResolveVideoUrlFromLinks(data.VideoLinks, row);
            var nextVideoUrl = string.IsNullOrWhiteSpace(videoUrlFromLinks)
                ? row.VideoUrl.Trim()
                : videoUrlFromLinks;

            if (existingVideo is null)
            {
                var videoId = await videoAdultRepository.CreateVideoAdult(new VideoAdult
                {
                    Source = normalizedSource,
                    ExternalId = row.ExternalId.Trim(),
                    VideoUrl = nextVideoUrl,
                    Title = string.IsNullOrWhiteSpace(row.Title) ? null : row.Title.Trim(),
                    ThumbnailUrl = string.IsNullOrWhiteSpace(row.ThumbnailUrl) ? null : row.ThumbnailUrl.Trim(),
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                });

                var createdVideo = await videoAdultRepository.GetVideoAdultById(videoId);
                if (createdVideo is null)
                {
                    invalid++;
                    continue;
                }

                await SyncVideoTagsAndActresses(createdVideo, row, actressByExcelId, actressByName);
                created++;

                videoByExternalId[createdVideo.ExternalId] = createdVideo;
                if (row.Id > 0)
                    videoByExcelId[row.Id] = createdVideo;
                continue;
            }

            var hasChanges = existingVideo.VideoUrl != nextVideoUrl
                || existingVideo.Title != (string.IsNullOrWhiteSpace(row.Title) ? null : row.Title.Trim())
                || existingVideo.ThumbnailUrl != (string.IsNullOrWhiteSpace(row.ThumbnailUrl) ? null : row.ThumbnailUrl.Trim())
                || existingVideo.Status != status;

            if (hasChanges)
            {
                existingVideo.VideoUrl = nextVideoUrl;
                existingVideo.Title = string.IsNullOrWhiteSpace(row.Title) ? existingVideo.Title : row.Title.Trim();
                existingVideo.ThumbnailUrl = string.IsNullOrWhiteSpace(row.ThumbnailUrl) ? existingVideo.ThumbnailUrl : row.ThumbnailUrl.Trim();
                existingVideo.Status = status;
                await videoAdultRepository.UpdateVideoAdult(existingVideo);
                updated++;
            }
            else
            {
                skipped++;
            }

            await SyncVideoTagsAndActresses(existingVideo, row, actressByExcelId, actressByName);
            videoByExternalId[existingVideo.ExternalId] = existingVideo;
            if (row.Id > 0)
                videoByExcelId[row.Id] = existingVideo;
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }

    private static List<int> ParseIds(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            return [];

        return csv.Split(',')
            .Select(x => x.Trim())
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .Where(x => x > 0)
            .Distinct()
            .ToList();
    }

    private static string ResolveVideoUrlFromLinks(List<VideoAdultLinkExcelRow> videoLinks, VideoAdultExcelRow videoRow)
    {
        var firstLink = videoLinks
            .Where(x => !string.IsNullOrWhiteSpace(x.Url)
                && ((videoRow.Id > 0 && x.VideoAdultId == videoRow.Id)
                    || (!string.IsNullOrWhiteSpace(videoRow.ExternalId)
                        && string.Equals(x.VideoExternalId?.Trim(), videoRow.ExternalId.Trim(), StringComparison.OrdinalIgnoreCase))))
            .OrderBy(x => x.OrderIndex <= 0 ? int.MaxValue : x.OrderIndex)
            .Select(x => x.Url.Trim())
            .FirstOrDefault();

        return firstLink ?? string.Empty;
    }

    private async Task<bool> SyncActressLinks(int actressId, List<string> urls)
    {
        var existingLinks = (await linkRepository.GetLinksByRefId(actressId, LinkType.ActressAdult)).ToList();
        var existingByUrl = existingLinks
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .ToDictionary(x => x.Url.Trim(), x => x, StringComparer.OrdinalIgnoreCase);
        var incoming = urls.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var changed = false;

        foreach (var existing in existingLinks)
        {
            if (!incoming.Contains(existing.Url.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                await linkRepository.DeleteLink(existing.Id);
                changed = true;
            }
        }

        for (int i = 0; i < incoming.Count; i++)
        {
            var url = incoming[i];
            var orderIndex = i + 1;

            if (existingByUrl.TryGetValue(url, out var current))
            {
                if (current.OrderIndex != orderIndex)
                {
                    current.OrderIndex = orderIndex;
                    await linkRepository.UpdateLink(current);
                    changed = true;
                }
            }
            else
            {
                await linkRepository.CreateLink(new Link
                {
                    Type = LinkType.ActressAdult,
                    RefId = actressId,
                    Url = url,
                    OrderIndex = orderIndex,
                    CreatedAt = DateTime.UtcNow
                });
                changed = true;
            }
        }

        return changed;
    }

    private async Task<bool> SyncVideoTagsAndActresses(
        VideoAdult video,
        VideoAdultExcelRow row,
        Dictionary<int, ActressAdult> actressByExcelId,
        Dictionary<string, ActressAdult> actressByName)
    {
        var changed = false;
        var tagIds = ParseIds(row.TagIds);
        if (tagIds.Count > 0)
        {
            await tagRepository.ReplaceTagsForRefId(video.Id, TagType.VideoAdult, tagIds);
            changed = true;
        }

        var actressIds = ParseIds(row.ActressIds);
        if (actressIds.Count == 0 && !string.IsNullOrWhiteSpace(row.ActressNames))
        {
            var names = row.ActressNames.Split(',')
                .Select(x => StringNormalizer.ToTitleCaseWithNumbers(x.Trim()))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            actressIds = names
                .Select(name => actressByName.TryGetValue(name, out var actress) ? actress.Id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();
        }
        else if (actressIds.Count > 0)
        {
            actressIds = actressIds
                .Select(id => actressByExcelId.TryGetValue(id, out var actress) ? actress.Id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();
        }

        if (actressIds.Count == 0)
            return changed;

        var existingActressIds = (await videoAdultRepository.GetActressIdsByVideoId(video.Id)).ToList();

        foreach (var removeId in existingActressIds.Where(id => !actressIds.Contains(id)))
        {
            await videoAdultRepository.RemoveActressFromVideo(video.Id, removeId);
            changed = true;
        }

        foreach (var addId in actressIds.Where(id => !existingActressIds.Contains(id)))
        {
            await videoAdultRepository.AddActressToVideo(video.Id, addId);
            changed = true;
        }

        return changed;
    }
}
