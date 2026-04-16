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

internal sealed class ImportActressJavExcelCommandHandler(IActressJavRepository repository, ILinkRepository linkRepository, ITagRepository tagRepository)
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

        var actressesById = new Dictionary<int, ActressJav>();
        var actressesByName = new Dictionary<string, ActressJav>(StringComparer.OrdinalIgnoreCase);
        var validTagIds = (await tagRepository.GetAllTagsByType(TagType.ActressJav)).Select(tag => tag.Id).ToHashSet();

        var existingActresses = await repository.GetAllActressJav();
        foreach (var actress in existingActresses)
        {
            actressesById[actress.Id] = actress;
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

            if (row.Id > 0 && actressesById.TryGetValue(row.Id, out var existingById))
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

                actressesById[actressId] = existing;
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
                    actressesById[existing.Id] = existing;
                    actressesByName[normalizedName] = existing;
                    actressUpdated = true;
                }
            }

            var desiredTagIds = ParseTagIds(row.Tags, validTagIds, out var hasTagInput, out var hadInvalidTagTokens);
            if (hadInvalidTagTokens)
                invalid++;

            var hasTagChanges = false;
            if (hasTagInput && desiredTagIds.Count > 0 && existing is not null)
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

        var linksByActressId = new Dictionary<int, List<ActressJavLinkExcelRow>>();
        foreach (var linkRow in excelData.Links)
        {
            var resolvedActress = ResolveActress(linkRow, actressesById, actressesByName);
            if (resolvedActress is null)
            {
                invalid++;
                continue;
            }

            if (!linksByActressId.TryGetValue(resolvedActress.Id, out var linkRows))
            {
                linkRows = new List<ActressJavLinkExcelRow>();
                linksByActressId[resolvedActress.Id] = linkRows;
            }

            linkRows.Add(linkRow);
        }

        foreach (var pair in linksByActressId)
        {
            await SyncLinksAsync(pair.Key, pair.Value, cancellationToken);
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
    }

    private ActressJav? ResolveActress(ActressJavLinkExcelRow linkRow, IDictionary<int, ActressJav> actressesById, IDictionary<string, ActressJav> actressesByName)
    {
        if (linkRow.ActressJavId > 0 && actressesById.TryGetValue(linkRow.ActressJavId, out var existingById))
            return existingById;

        var actressName = StringNormalizer.ToTitleCase(linkRow.ActressJavName ?? string.Empty);
        if (!string.IsNullOrWhiteSpace(actressName) && actressesByName.TryGetValue(actressName, out var existingByName))
            return existingByName;

        return null;
    }

    private async Task<bool> SyncLinksAsync(int actressId, List<ActressJavLinkExcelRow> links, CancellationToken cancellationToken)
    {
        if (links is null)
            return false;

        await linkRepository.DeleteLinksByRefId(actressId, LinkType.ActressJav);

        var orderedLinks = links
            .Where(link => !string.IsNullOrWhiteSpace(link.Url))
            .OrderBy(link => link.OrderIndex > 0 ? link.OrderIndex : int.MaxValue)
            .ToList();

        for (int i = 0; i < orderedLinks.Count; i++)
        {
            var url = orderedLinks[i].Url.Trim();
            if (string.IsNullOrWhiteSpace(url))
                continue;

            await linkRepository.CreateLink(new Link
            {
                Type = LinkType.ActressJav,
                RefId = actressId,
                Url = url,
                OrderIndex = i + 1,
                CreatedAt = DateTime.UtcNow
            });
        }

        return true;
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
