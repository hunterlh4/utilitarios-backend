using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record ImportActressJavExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportActressJavExcelCommandHandler(
    IActressJavRepository repository,
    IJavRepository javRepository,
    ILinkJavRepository linkJavRepository,
    ILinkActressJavRepository linkActressJavRepository,
    ITagRepository tagRepository)
    : IRequestHandler<ImportActressJavExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportActressJavExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var excelData = ExcelHelper.ReadActressJavExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var actressesByExcelId = new Dictionary<int, ActressJav>();
        var actressesByName = new Dictionary<string, ActressJav>(StringComparer.OrdinalIgnoreCase);
        var validActressTagIds = (await tagRepository.GetAllTagsByType(TagType.ActressJav)).Select(tag => tag.Id).ToHashSet();
        var validJavTagIds = (await tagRepository.GetAllTagsByType(TagType.Jav)).Select(tag => tag.Id).ToHashSet();

        var existingActresses = await repository.GetAllActressJav();
        foreach (var actress in existingActresses)
        {
            actressesByExcelId[actress.Id] = actress;
            actressesByName[StringNormalizer.ToTitleCase(actress.Name)] = actress;
        }

        foreach (var row in excelData.Actresses)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToTitleCase(row.Name);
            ActressJav? existing = null;
            var wasCreated = false;
            var actressUpdated = false;

            if (row.Id > 0 && actressesByExcelId.TryGetValue(row.Id, out var existingById))
            {
                existing = existingById;
            }
            else if (actressesByName.TryGetValue(normalizedName, out var existingByName))
            {
                existing = existingByName;
            }

            if (existing is null)
            {
                var actressId = await repository.CreateActressJav(new ActressJav
                {
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                });

                existing = new ActressJav
                {
                    Id = actressId,
                    Name = normalizedName,
                    Image = string.IsNullOrWhiteSpace(row.Image) ? null : row.Image,
                    CreatedAt = DateTime.UtcNow,
                };

                actressesByExcelId[actressId] = existing;
                actressesByName[normalizedName] = existing;
                wasCreated = true;
            }
            else
            {
                var nextImage = string.IsNullOrWhiteSpace(row.Image) ? existing.Image : row.Image;
                var hasActressChanges = existing.Name != normalizedName || existing.Image != nextImage;

                if (hasActressChanges)
                {
                    existing.Name = normalizedName;
                    existing.Image = nextImage;
                    await repository.UpdateActressJav(existing);
                    actressesByExcelId[existing.Id] = existing;
                    actressesByName[normalizedName] = existing;
                    actressUpdated = true;
                }
            }

            if (existing is not null && row.Id > 0)
                actressesByExcelId[row.Id] = existing;

            var desiredTagIds = ParseTagIds(row.TagIds, validActressTagIds, out var hasTagInput, out var hadInvalidTagTokens);
            if (hadInvalidTagTokens)
                invalid++;

            var hasTagChanges = false;
            if (hasTagInput && existing is not null)
            {
                var currentTagIds = (await tagRepository.GetTagsByRefId(existing.Id, TagType.ActressJav))
                    .Select(tag => tag.Id)
                    .ToHashSet();

                if (!currentTagIds.SetEquals(desiredTagIds))
                {
                    await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.ActressJav, desiredTagIds);
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

        var actressLinksByActressId = new Dictionary<int, List<string>>();
        foreach (var linkRow in excelData.Links.Where(x => !string.IsNullOrWhiteSpace(x.Url)))
        {
            var resolvedActress = ResolveActress(linkRow, actressesByExcelId, actressesByName);
            if (resolvedActress is null)
            {
                invalid++;
                continue;
            }

            if (!actressLinksByActressId.TryGetValue(resolvedActress.Id, out var urls))
            {
                urls = new List<string>();
                actressLinksByActressId[resolvedActress.Id] = urls;
            }

            urls.Add(linkRow.Url.Trim());
        }

        foreach (var pair in actressLinksByActressId)
        {
            await SyncActressLinks(pair.Key, pair.Value);
        }

        var javsByExcelId = new Dictionary<int, Jav>();
        var javsByCode = new Dictionary<string, Jav>(StringComparer.OrdinalIgnoreCase);
        var existingJavs = await javRepository.GetAllJavs();
        foreach (var jav in existingJavs)
        {
            javsByExcelId[jav.Id] = jav;
            javsByCode[jav.Code] = jav;
        }

        foreach (var row in excelData.Javs)
        {
            if (string.IsNullOrWhiteSpace(row.Code))
            {
                invalid++;
                continue;
            }

            var normalizedCode = row.Code.Trim().ToUpperInvariant();
            Jav? existingJav = null;
            var wasCreated = false;

            if (row.Id > 0 && javsByExcelId.TryGetValue(row.Id, out var existingJavById))
            {
                existingJav = existingJavById;
            }
            else if (javsByCode.TryGetValue(normalizedCode, out var existingJavByCode))
            {
                existingJav = existingJavByCode;
            }

            var nextImage = string.IsNullOrWhiteSpace(row.Image) ? string.Empty : row.Image.Trim();
            if (existingJav is null && string.IsNullOrWhiteSpace(nextImage))
            {
                invalid++;
                continue;
            }

            var status = Enum.IsDefined(typeof(ContentStatus), row.Status)
                ? (ContentStatus)row.Status
                : ContentStatus.Pending;

            if (existingJav is null)
            {
                var javId = await javRepository.CreateJav(new Jav
                {
                    Code = normalizedCode,
                    Image = nextImage,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                });

                existingJav = await javRepository.GetJavById(javId);
                if (existingJav is null)
                {
                    invalid++;
                    continue;
                }

                wasCreated = true;
            }

            var hasJavChanges = existingJav.Code != normalizedCode
                || (!string.IsNullOrWhiteSpace(nextImage) && existingJav.Image != nextImage)
                || existingJav.Status != status;

            if (hasJavChanges)
            {
                existingJav.Code = normalizedCode;
                existingJav.Image = string.IsNullOrWhiteSpace(nextImage) ? existingJav.Image : nextImage;
                existingJav.Status = status;
                await javRepository.UpdateJav(existingJav);
            }

            javsByExcelId[existingJav.Id] = existingJav;
            javsByCode[existingJav.Code] = existingJav;
            if (row.Id > 0)
                javsByExcelId[row.Id] = existingJav;

            var desiredJavTagIds = ParseTagIds(row.TagIds, validJavTagIds, out var hasJavTagInput, out var hadInvalidJavTagTokens);
            if (hadInvalidJavTagTokens)
                invalid++;

            var hasJavTagChanges = false;
            if (hasJavTagInput)
            {
                var currentJavTagIds = (await tagRepository.GetTagsByRefId(existingJav.Id, TagType.Jav))
                    .Select(tag => tag.Id)
                    .ToHashSet();

                if (!currentJavTagIds.SetEquals(desiredJavTagIds))
                {
                    await tagRepository.ReplaceTagsForRefId(existingJav.Id, TagType.Jav, desiredJavTagIds);
                    hasJavTagChanges = true;
                }
            }

            var actressIds = ResolveActressIdsForJav(row, actressesByExcelId);
            var hasActressRelationChanges = false;
            if (actressIds.Count > 0)
            {
                var currentActressIds = (await javRepository.GetActressIdsByJavId(existingJav.Id)).ToList();

                foreach (var removeId in currentActressIds.Where(id => !actressIds.Contains(id)))
                {
                    await javRepository.RemoveActressFromJav(existingJav.Id, removeId);
                    hasActressRelationChanges = true;
                }

                foreach (var addId in actressIds.Where(id => !currentActressIds.Contains(id)))
                {
                    await javRepository.AddActressToJav(existingJav.Id, addId);
                    hasActressRelationChanges = true;
                }
            }

            var javUrls = ResolveJavUrls(row, excelData.JavLinks);
            var hasJavLinkChanges = false;
            if (javUrls.Count > 0)
                hasJavLinkChanges = await SyncJavLinks(existingJav.Id, javUrls);

            if (wasCreated)
                created++;
            else if (hasJavChanges || hasJavTagChanges || hasActressRelationChanges || hasJavLinkChanges)
                updated++;
            else
                skipped++;
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }

    private ActressJav? ResolveActress(ActressJavLinkExcelRow linkRow, IDictionary<int, ActressJav> actressesByExcelId, IDictionary<string, ActressJav> actressesByName)
    {
        if (linkRow.ActressJavId > 0 && actressesByExcelId.TryGetValue(linkRow.ActressJavId, out var existingById))
            return existingById;

        var actressName = StringNormalizer.ToTitleCase(linkRow.ActressJavName ?? string.Empty);
        if (!string.IsNullOrWhiteSpace(actressName) && actressesByName.TryGetValue(actressName, out var existingByName))
            return existingByName;

        return null;
    }

    private async Task<bool> SyncActressLinks(int actressId, List<string> urls)
    {
        var existingLinks = (await linkActressJavRepository.GetLinkActressJavsByActressId(actressId)).ToList();
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
                await linkActressJavRepository.DeleteLinkActressJav(existing.Id);
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
                    await linkActressJavRepository.UpdateLinkActressJav(current);
                    changed = true;
                }
            }
            else
            {
                await linkActressJavRepository.CreateLinkActressJav(new LinkActressJav
                {
                    ActressJavId = actressId,
                    Url = url,
                    OrderIndex = orderIndex,
                    CreatedAt = DateTime.UtcNow
                });
                changed = true;
            }
        }

        return changed;
    }

    private async Task<bool> SyncJavLinks(int javId, List<string> urls)
    {
        var existingLinks = (await linkJavRepository.GetLinkJavsByJavId(javId)).ToList();
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
                await linkJavRepository.DeleteLinkJav(existing.Id);
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
                    await linkJavRepository.UpdateLinkJav(current);
                    changed = true;
                }
            }
            else
            {
                await linkJavRepository.CreateLinkJav(new LinkJav
                {
                    JavId = javId,
                    Url = url,
                    OrderIndex = orderIndex,
                    CreatedAt = DateTime.UtcNow
                });
                changed = true;
            }
        }

        return changed;
    }

    private static List<int> ResolveActressIdsForJav(
        JavExcelRow row,
        Dictionary<int, ActressJav> actressesByExcelId)
    {
        var actressIds = ParseIds(row.ActressIds);

        if (actressIds.Count > 0)
        {
            actressIds = actressIds
                .Select(id => actressesByExcelId.TryGetValue(id, out var actress) ? actress.Id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();
        }

        return actressIds;
    }

    private static List<string> ResolveJavUrls(JavExcelRow row, List<JavLinkExcelRow> javLinks)
    {
        var normalizedCode = string.IsNullOrWhiteSpace(row.Code) ? string.Empty : row.Code.Trim().ToUpperInvariant();

        return javLinks
            .Where(x => !string.IsNullOrWhiteSpace(x.Url)
                && ((row.Id > 0 && x.JavId == row.Id)
                    || (!string.IsNullOrWhiteSpace(normalizedCode)
                        && string.Equals(x.JavCode?.Trim(), normalizedCode, StringComparison.OrdinalIgnoreCase))))
            .OrderBy(x => x.OrderIndex <= 0 ? int.MaxValue : x.OrderIndex)
            .Select(x => x.Url.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
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
