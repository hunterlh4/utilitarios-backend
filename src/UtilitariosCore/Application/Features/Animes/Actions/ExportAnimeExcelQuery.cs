using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Animes.Actions;

public record ExportAnimeExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportAnimeExcelQueryHandler(IAnimeRepository repository)
    : IRequestHandler<ExportAnimeExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportAnimeExcelQuery request, CancellationToken cancellationToken)
    {
        var animes = (await repository.GetAllAnimes())
            .Select(a => new AnimeExcelRow
            {
                Id = a.Id,
                ApiId = a.ApiId,
                Title = a.Title,
                Image = a.Image,
                Episodes = a.Episodes,
                Status = (int)a.Status,
            })
            .ToList();

        using var stream = ExcelHelper.CreateAnimeExcel(animes);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"anime-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
