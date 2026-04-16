using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record ExportActressJavExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportActressJavExcelQueryHandler(
    IActressJavRepository repository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<ExportActressJavExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressJavExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressJav()).ToList();
        var exportRows = new List<ActressJavExcelRow>();

        var linkRows = new List<ActressJavLinkExcelRow>();

        foreach (var actress in actresses)
        {
            var tags = await tagRepository.GetTagsByRefId(actress.Id, TagType.ActressJav);
            var tagIds = tags
                .Select(tag => tag.Id)
                .Where(id => id > 0)
                .OrderBy(id => id)
                .ToList();

            exportRows.Add(new ActressJavExcelRow
            {
                Id = actress.Id,
                Name = actress.Name,
                Image = actress.Image,
                Tags = tagIds.Count > 0 ? string.Join(", ", tagIds) : string.Empty,
            });

            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressJav);
            foreach (var link in links)
            {
                linkRows.Add(new ActressJavLinkExcelRow
                {
                    ActressJavId = actress.Id,
                    ActressJavName = actress.Name,
                    Url = link.Url,
                    OrderIndex = link.OrderIndex ?? 0,
                });
            }
        }

        using var stream = ExcelHelper.CreateActressJavExcel(exportRows, linkRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"actress-jav-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
