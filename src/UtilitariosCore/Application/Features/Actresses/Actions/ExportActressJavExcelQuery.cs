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
    IJavRepository javRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<ExportActressJavExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressJavExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressJav())
            .OrderBy(actress => actress.Id)
            .ToList();
        var exportRows = new List<ActressJavExcelRow>();

        var linkRows = new List<ActressJavLinkExcelRow>();
        var javRows = new List<JavExcelRow>();
        var javLinkRows = new List<JavLinkExcelRow>();

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
                TagIds = tagIds.Count > 0 ? string.Join(',', tagIds) : null,
            });

            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressJav);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
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

        var javs = (await javRepository.GetAllJavs())
            .OrderBy(jav => jav.Id)
            .ToList();
        foreach (var jav in javs)
        {
            var javTagIds = (await tagRepository.GetTagsByRefId(jav.Id, TagType.Jav))
                .Select(t => t.Id)
                .Where(id => id > 0)
                .OrderBy(id => id)
                .ToList();
            var actressIds = (await javRepository.GetActressIdsByJavId(jav.Id))
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            javRows.Add(new JavExcelRow
            {
                Id = jav.Id,
                Code = jav.Code,
                Image = jav.Image,
                Status = (int)jav.Status,
                TagIds = javTagIds.Count > 0 ? string.Join(',', javTagIds) : null,
                ActressIds = actressIds.Count > 0 ? string.Join(',', actressIds) : null,
            });

            var links = await linkRepository.GetLinksByRefId(jav.Id, LinkType.Jav);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
            {
                javLinkRows.Add(new JavLinkExcelRow
                {
                    JavId = jav.Id,
                    JavCode = jav.Code,
                    Url = link.Url,
                    OrderIndex = link.OrderIndex ?? 0,
                });
            }
        }

        using var stream = ExcelHelper.CreateActressJavExcel(exportRows, linkRows, javRows, javLinkRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"actress-jav-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
