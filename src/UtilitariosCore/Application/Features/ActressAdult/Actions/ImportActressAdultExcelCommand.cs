using MediatR;
using OfficeOpenXml;
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

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var stream = new MemoryStream(request.FileBytes);
        using var package = new ExcelPackage(stream);
        
        var actressSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressAdult")
                        ?? package.Workbook.Worksheets.FirstOrDefault();
        var actressLinksSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "ActressAdultLinks");
        var videosSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Videos");
        var relationsSheet = package.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Relations");

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var actressesByExcelId = new Dictionary<int, Domain.Models.ActressAdult>();
        var actressesByName = new Dictionary<string, Domain.Models.ActressAdult>(StringComparer.OrdinalIgnoreCase);
        var validActressTagIds = (await tagRepository.GetAllTagsByType(TagType.ActressAdult)).Select(tag => tag.Id).ToHashSet();
        var validVideoTagIds = (await tagRepository.GetAllTagsByType(TagType.VideoAdult)).Select(tag => tag.Id).ToHashSet();

        var existingActresses = await repository.GetAllActressAdults();
        foreach (var actress in existingActresses)
        {
            actressesByExcelId[actress.Id] = actress;
            actressesByName[StringNormalizer.ToTitleCaseWithNumbers(actress.Name)] = actress;
        }

        // Procesar actrices
        if (actressSheet?.Dimension is not null)
        {
            for (int row = 2; row <= actressSheet.Dimension.End.Row; row++)
            {
                var idText = actressSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var name = actressSheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var image = actressSheet.Dimension.End.Column >= 3 ? actressSheet.Cells[row, 3].Text?.Trim() : null;
                var tagIds = actressSheet.Dimension.End.Column >= 4 ? actressSheet.Cells[row, 4].Text?.Trim() : null;

                if (string.IsNullOrWhiteSpace(name))
                {
                    invalid++;
                    continue;
                }

                int.TryParse(idText, out var excelId);
                var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(name);
                Domain.Models.ActressAdult? existing = null;
                var wasCreated = false;
                var actressUpdated = false;

                // Buscar primero por ID si es válido
                if (excelId > 0 && actressesByExcelId.TryGetValue(excelId, out var existingById))
                {
                    existing = existingById;
                }
                else if (actressesByName.TryGetValue(normalizedName, out var existingByName))
                {
                    existing = existingByName;
                }

                if (existing is null)
                {
                    var actressId = await repository.CreateActressAdult(new Domain.Models.ActressAdult
                    {
                        Name = normalizedName,
                        Image = string.IsNullOrWhiteSpace(image) ? null : image,
                        CreatedAt = DateTime.UtcNow,
                    });

                    existing = new Domain.Models.ActressAdult
                    {
                        Id = actressId,
                        Name = normalizedName,
                        Image = string.IsNullOrWhiteSpace(image) ? null : image,
                        CreatedAt = DateTime.UtcNow,
                    };

                    actressesByExcelId[actressId] = existing;
                    actressesByName[normalizedName] = existing;
                    if (excelId > 0)
                        actressesByExcelId[excelId] = existing;
                    wasCreated = true;
                }
                else
                {
                    var nextImage = string.IsNullOrWhiteSpace(image) ? existing.Image : image;
                    var hasActressChanges = existing.Name != normalizedName || existing.Image != nextImage;

                    if (hasActressChanges)
                    {
                        existing.Name = normalizedName;
                        existing.Image = nextImage;
                        await repository.UpdateActressAdult(existing);
                        actressesByExcelId[existing.Id] = existing;
                        actressesByName[normalizedName] = existing;
                        if (excelId > 0)
                            actressesByExcelId[excelId] = existing;
                        actressUpdated = true;
                    }
                }

                var desiredTagIds = ParseTagIds(tagIds, validActressTagIds, out var hasTagInput, out var hadInvalidTagTokens);
                if (hadInvalidTagTokens)
                    invalid++;

                var hasTagChanges = false;
                if (hasTagInput && existing is not null)
                {
                    var currentTagIds = (await tagRepository.GetTagsByRefId(existing.Id, TagType.ActressAdult))
                        .Select(tag => tag.Id)
                        .ToHashSet();

                    if (!currentTagIds.SetEquals(desiredTagIds))
                    {
                        await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.ActressAdult, desiredTagIds);
                        hasTagChanges = true;
                    }
                }

                if (wasCreated)
                    created++;
                else if (actressUpdated || hasTagChanges)
                    updated++;
                else
                    skipped++;
            }
        }

        // Procesar links de actrices
        var actressLinksByActressId = new Dictionary<int, List<string>>();
        if (actressLinksSheet?.Dimension is not null)
        {
            for (int row = 2; row <= actressLinksSheet.Dimension.End.Row; row++)
            {
                var actressIdText = actressLinksSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var actressName = actressLinksSheet.Cells[row, 2].Text?.Trim();
                var url = actressLinksSheet.Cells[row, 3].Text?.Trim();

                if (string.IsNullOrWhiteSpace(url))
                    continue;

                Domain.Models.ActressAdult? actress = null;

                // Buscar primero por ID
                if (int.TryParse(actressIdText, out var actressId) && actressId > 0 && actressesByExcelId.TryGetValue(actressId, out var actressByIdResult))
                {
                    actress = actressByIdResult;
                }
                // Si no se encuentra por ID, buscar por nombre
                else if (!string.IsNullOrWhiteSpace(actressName))
                {
                    var normalizedName = StringNormalizer.ToTitleCaseWithNumbers(actressName);
                    actressesByName.TryGetValue(normalizedName, out actress);
                }

                if (actress is not null)
                {
                    if (!actressLinksByActressId.TryGetValue(actress.Id, out var urls))
                    {
                        urls = new List<string>();
                        actressLinksByActressId[actress.Id] = urls;
                    }
                    urls.Add(url);
                }
            }
        }

        foreach (var pair in actressLinksByActressId)
        {
            await SyncActressLinks(pair.Key, pair.Value);
        }

        // Procesar videos
        var videosByExcelId = new Dictionary<int, VideoAdult>();
        var videosByExternalId = new Dictionary<string, VideoAdult>(StringComparer.OrdinalIgnoreCase);
        var existingVideos = await videoAdultRepository.GetAllVideoAdults();
        foreach (var video in existingVideos)
        {
            videosByExcelId[video.Id] = video;
            if (!string.IsNullOrWhiteSpace(video.ExternalId))
                videosByExternalId[video.ExternalId] = video;
        }

        if (videosSheet?.Dimension is not null)
        {
            for (int row = 2; row <= videosSheet.Dimension.End.Row; row++)
            {
                var idText = videosSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var source = videosSheet.Cells[row, 2].Text?.Trim() ?? string.Empty;
                var externalId = videosSheet.Cells[row, 3].Text?.Trim() ?? string.Empty;
                var videoUrl = videosSheet.Dimension.End.Column >= 4 ? videosSheet.Cells[row, 4].Text?.Trim() : null;
                var title = videosSheet.Dimension.End.Column >= 5 ? videosSheet.Cells[row, 5].Text?.Trim() : null;
                var thumbnailUrl = videosSheet.Dimension.End.Column >= 6 ? videosSheet.Cells[row, 6].Text?.Trim() : null;
                var statusText = videosSheet.Dimension.End.Column >= 7 ? videosSheet.Cells[row, 7].Text?.Trim() ?? "0" : "0";
                var tagIds = videosSheet.Dimension.End.Column >= 8 ? videosSheet.Cells[row, 8].Text?.Trim() : null;

                if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(externalId))
                {
                    invalid++;
                    continue;
                }

                int.TryParse(idText, out var excelId);
                var normalizedSource = source.Trim().ToLowerInvariant();
                VideoAdult? existingVideo = null;
                var wasCreated = false;

                // Buscar primero por ID si es válido
                if (excelId > 0 && videosByExcelId.TryGetValue(excelId, out var existingById))
                {
                    existingVideo = existingById;
                }
                else if (videosByExternalId.TryGetValue(externalId, out var existingByExternalId))
                {
                    existingVideo = existingByExternalId;
                }

                var status = int.TryParse(statusText, out var statusValue) && Enum.IsDefined(typeof(ContentStatus), statusValue)
                    ? (ContentStatus)statusValue
                    : ContentStatus.Pending;

                if (existingVideo is null)
                {
                    var videoId = await videoAdultRepository.CreateVideoAdult(new VideoAdult
                    {
                        Source = normalizedSource,
                        ExternalId = externalId,
                        VideoUrl = videoUrl ?? string.Empty,
                        Title = title,
                        ThumbnailUrl = thumbnailUrl,
                        Status = status,
                        CreatedAt = DateTime.UtcNow,
                    });

                    existingVideo = new VideoAdult
                    {
                        Id = videoId,
                        Source = normalizedSource,
                        ExternalId = externalId,
                        VideoUrl = videoUrl ?? string.Empty,
                        Title = title,
                        ThumbnailUrl = thumbnailUrl,
                        Status = status,
                        CreatedAt = DateTime.UtcNow,
                    };

                    videosByExcelId[videoId] = existingVideo;
                    videosByExternalId[externalId] = existingVideo;
                    if (excelId > 0)
                        videosByExcelId[excelId] = existingVideo;
                    wasCreated = true;
                }
                else
                {
                    var hasVideoChanges = existingVideo.VideoUrl != (videoUrl ?? string.Empty)
                        || existingVideo.Title != title
                        || existingVideo.ThumbnailUrl != thumbnailUrl
                        || existingVideo.Status != status;

                    if (hasVideoChanges)
                    {
                        existingVideo.VideoUrl = videoUrl ?? string.Empty;
                        existingVideo.Title = title;
                        existingVideo.ThumbnailUrl = thumbnailUrl;
                        existingVideo.Status = status;
                        await videoAdultRepository.UpdateVideoAdult(existingVideo);
                    }
                }

                var desiredVideoTagIds = ParseTagIds(tagIds, validVideoTagIds, out var hasVideoTagInput, out var hadInvalidVideoTagTokens);
                if (hadInvalidVideoTagTokens)
                    invalid++;

                var hasVideoTagChanges = false;
                if (hasVideoTagInput)
                {
                    var currentVideoTagIds = (await tagRepository.GetTagsByRefId(existingVideo.Id, TagType.VideoAdult))
                        .Select(tag => tag.Id)
                        .ToHashSet();

                    if (!currentVideoTagIds.SetEquals(desiredVideoTagIds))
                    {
                        await tagRepository.ReplaceTagsForRefId(existingVideo.Id, TagType.VideoAdult, desiredVideoTagIds);
                        hasVideoTagChanges = true;
                    }
                }

                if (wasCreated)
                    created++;
                else if (hasVideoTagChanges)
                    updated++;
                else
                    skipped++;
            }
        }

        // Procesar relaciones video-actriz
        if (relationsSheet?.Dimension is not null)
        {
            for (int row = 2; row <= relationsSheet.Dimension.End.Row; row++)
            {
                var videoIdText = relationsSheet.Cells[row, 1].Text?.Trim() ?? "0";
                var actressIdText = relationsSheet.Cells[row, 2].Text?.Trim() ?? "0";
                var externalId = relationsSheet.Cells[row, 3].Text?.Trim();
                var actressName = relationsSheet.Cells[row, 4].Text?.Trim();

                VideoAdult? video = null;
                Domain.Models.ActressAdult? actress = null;

                // Buscar video primero por ID, luego por ExternalId
                if (int.TryParse(videoIdText, out var videoId) && videoId > 0 && videosByExcelId.TryGetValue(videoId, out var videoByIdResult))
                {
                    video = videoByIdResult;
                }
                else if (!string.IsNullOrWhiteSpace(externalId) && videosByExternalId.TryGetValue(externalId, out var videoByExternalIdResult))
                {
                    video = videoByExternalIdResult;
                }

                // Buscar actriz primero por ID, luego por nombre
                if (int.TryParse(actressIdText, out var actressId) && actressId > 0 && actressesByExcelId.TryGetValue(actressId, out var actressByIdResult))
                {
                    actress = actressByIdResult;
                }
                else if (!string.IsNullOrWhiteSpace(actressName))
                {
                    var normalizedActressName = StringNormalizer.ToTitleCaseWithNumbers(actressName);
                    actressesByName.TryGetValue(normalizedActressName, out actress);
                }

                if (video is not null && actress is not null)
                {
                    var currentActressIds = (await videoAdultRepository.GetActressIdsByVideoId(video.Id)).ToList();
                    if (!currentActressIds.Contains(actress.Id))
                    {
                        await videoAdultRepository.AddActressToVideo(video.Id, actress.Id);
                    }
                }
            }
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }

    private async Task<bool> SyncActressLinks(int actressId, List<string> urls)
    {
        var existingLinks = (await linkRepository.GetLinksByRefId(actressId, LinkType.ActressAdult)).ToList();
        var existingByUrl = existingLinks
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .ToDictionary(x => x.Url.Trim(), x => x, StringComparer.OrdinalIgnoreCase);
        var incoming = urls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => url.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
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

    private static List<int> ParseTagIds(string? rawTags, HashSet<int> validTagIds, out bool hasInput, out bool hadInvalidTokens)
    {
        hasInput = !string.IsNullOrWhiteSpace(rawTags);
        hadInvalidTokens = false;

        if (!hasInput)
            return [];

        var tagIds = new HashSet<int>();
        var tokens = (rawTags ?? string.Empty).Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var token in tokens)
        {
            if (int.TryParse(token, out var tagId) && tagId > 0 && validTagIds.Contains(tagId))
            {
                tagIds.Add(tagId);
                continue;
            }

            hadInvalidTokens = true;
        }

        return tagIds.OrderBy(id => id).ToList();
    }
}