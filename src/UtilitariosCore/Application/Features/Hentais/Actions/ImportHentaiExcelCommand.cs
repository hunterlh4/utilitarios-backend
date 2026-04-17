using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record ImportHentaiExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportHentaiExcelCommandHandler(IHentaiRepository repository, ITagRepository tagRepository)
    : IRequestHandler<ImportHentaiExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportHentaiExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var rows = ExcelHelper.ReadHentaiExcel(stream);
        var validTagIds = (await tagRepository.GetAllTagsByType(TagType.Hentai)).Select(tag => tag.Id).ToHashSet();

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.ApiId) || string.IsNullOrWhiteSpace(row.Title))
            {
                invalid++;
                continue;
            }

            var existing = await repository.GetHentaiByApiId(row.ApiId.Trim());
            var status = Enum.IsDefined(typeof(ContentStatus), row.Status)
                ? (ContentStatus)row.Status
                : ContentStatus.Pending;
            var desiredTagIds = ParseTagIds(row.TagIds, validTagIds, out var hasTagInput, out var hadInvalidTagTokens);

            if (hadInvalidTagTokens)
                invalid++;

            if (existing is null)
            {
                var hentaiId = await repository.CreateHentai(new Hentai
                {
                    ApiId = row.ApiId.Trim(),
                    Title = row.Title.Trim(),
                    Image = row.Image?.Trim() ?? string.Empty,
                    Episodes = row.Episodes,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                });

                if (hasTagInput)
                    await tagRepository.ReplaceTagsForRefId(hentaiId, TagType.Hentai, desiredTagIds);

                created++;
                continue;
            }

            var nextTitle = row.Title.Trim();
            var nextImage = row.Image?.Trim() ?? existing.Image;
            var hasChanges =
                existing.Title != nextTitle ||
                existing.Image != nextImage ||
                existing.Episodes != row.Episodes ||
                existing.Status != status;

            var tagUpdated = false;
            if (hasTagInput)
            {
                var currentTagIds = (await tagRepository.GetTagsByRefId(existing.Id, TagType.Hentai))
                    .Select(tag => tag.Id)
                    .ToHashSet();

                if (!currentTagIds.SetEquals(desiredTagIds))
                {
                    await tagRepository.ReplaceTagsForRefId(existing.Id, TagType.Hentai, desiredTagIds);
                    tagUpdated = true;
                }
            }

            if (!hasChanges && !tagUpdated)
            {
                skipped++;
                continue;
            }

            if (hasChanges)
            {
                existing.Title = nextTitle;
                existing.Image = nextImage;
                existing.Episodes = row.Episodes;
                existing.Status = status;
                await repository.UpdateHentai(existing);
            }

            updated++;
        }

        return new ImportExcelResult
        {
            Created = created,
            Updated = updated,
            Skipped = skipped,
            Invalid = invalid
        };
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
            }
            else
            {
                hadInvalidTokens = true;
            }
        }

        return tagIds.OrderBy(id => id).ToList();
    }
}
