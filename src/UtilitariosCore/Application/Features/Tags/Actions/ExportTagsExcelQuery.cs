using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Tags.Actions;

public record ExportTagsExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportTagsExcelQueryHandler(ITagRepository tagRepository)
    : IRequestHandler<ExportTagsExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportTagsExcelQuery request, CancellationToken cancellationToken)
    {
        var tags = new List<Tag>();

        foreach (var type in Enum.GetValues<TagType>())
        {
            var typeTags = await tagRepository.GetAllTagsByType(type);
            tags.AddRange(typeTags);
        }

        var exportRows = tags.Select(tag => new TagExcelRow
        {
            Id = tag.Id,
            Name = tag.Name,
            Type = (int)tag.Type,
        }).ToList();

        using var stream = ExcelHelper.CreateTagExcel(exportRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"tags.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}