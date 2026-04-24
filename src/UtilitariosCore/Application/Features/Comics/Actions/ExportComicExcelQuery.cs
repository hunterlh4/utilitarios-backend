using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public record ExportComicExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportComicExcelQueryHandler(IComicRepository repository)
    : IRequestHandler<ExportComicExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportComicExcelQuery request, CancellationToken cancellationToken)
    {
        var comics = (await repository.GetAllComics()).ToList();
        var rows = comics.Select(c => new ComicExcelRow
        {
            Id = c.Id,
            Name = c.Name,
            Image = c.Image,
            Url = c.Url,
            Category = c.Category,
        }).ToList();

        using var stream = ExcelHelper.CreateComicExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"comic-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
