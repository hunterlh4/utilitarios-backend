using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record ExportHentaiExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportHentaiExcelQueryHandler(IHentaiRepository repository, ITagRepository tagRepository)
    : IRequestHandler<ExportHentaiExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportHentaiExcelQuery request, CancellationToken cancellationToken)
    {
        var hentais = (await repository.GetAllHentais()).ToList();
        var rows = new List<HentaiExcelRow>();

        foreach (var hentai in hentais)
        {
            var tagIds = (await tagRepository.GetTagsByRefId(hentai.Id, TagType.Hentai))
                .Select(tag => tag.Id)
                .Where(id => id > 0)
                .OrderBy(id => id)
                .ToList();

            rows.Add(new HentaiExcelRow
            {
                Id = hentai.Id,
                ApiId = hentai.ApiId,
                Title = hentai.Title,
                Image = hentai.Image,
                Episodes = hentai.Episodes,
                Status = (int)hentai.Status,
                TagIds = tagIds.Count > 0 ? string.Join(", ", tagIds) : string.Empty,
            });
        }

        using var stream = ExcelHelper.CreateHentaiExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"hentai-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
