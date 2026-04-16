using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record ImportTagsExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportTagsExcelCommandHandler(ITagRepository tagRepository)
    : IRequestHandler<ImportTagsExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportTagsExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacio.");

        using var stream = new MemoryStream(request.FileBytes);
        var excelData = ExcelHelper.ReadTagExcel(stream);

        int created = 0;
        int updated = 0;
        int skipped = 0;
        int invalid = 0;

        var existingTags = new List<Tag>();
        foreach (var type in Enum.GetValues<TagType>())
        {
            existingTags.AddRange(await tagRepository.GetAllTagsByType(type));
        }

        var tagsById = existingTags.ToDictionary(tag => tag.Id, tag => tag);
        var tagsByNameAndType = existingTags.ToDictionary(tag => ($"{tag.Type}:{tag.Name}"), tag => tag, StringComparer.OrdinalIgnoreCase);

        foreach (var row in excelData.Tags)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                invalid++;
                continue;
            }

            var normalizedName = StringNormalizer.ToNormalizedTag(row.Name);
            var tagType = Enum.IsDefined(typeof(TagType), row.Type) ? (TagType)row.Type : TagType.Other;
            Tag? existing = null;

            if (row.Id > 0 && tagsById.TryGetValue(row.Id, out var existingById))
            {
                existing = existingById;
            }
            else if (tagsByNameAndType.TryGetValue($"{tagType}:{normalizedName}", out var existingByName))
            {
                existing = existingByName;
            }

            if (existing is null)
            {
                var id = await tagRepository.CreateTag(new Tag
                {
                    Name = normalizedName,
                    Type = tagType
                });

                var createdTag = new Tag
                {
                    Id = id,
                    Name = normalizedName,
                    Type = tagType
                };

                tagsById[id] = createdTag;
                tagsByNameAndType[$"{tagType}:{normalizedName}"] = createdTag;
                created++;
                continue;
            }

            if (existing.Name == normalizedName)
            {
                skipped++;
                continue;
            }

            existing.Name = normalizedName;
            await tagRepository.UpdateTag(existing);
            tagsById[existing.Id] = existing;
            tagsByNameAndType[$"{tagType}:{normalizedName}"] = existing;
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
}