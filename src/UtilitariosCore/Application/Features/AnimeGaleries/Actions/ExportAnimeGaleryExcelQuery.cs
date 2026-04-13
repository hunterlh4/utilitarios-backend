using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.AnimeGaleries.Actions;

public record ExportAnimeGaleryExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportAnimeGaleryExcelQueryHandler(
    IGaleryRepository repository,
    IMediaRepository mediaRepository)
    : IRequestHandler<ExportAnimeGaleryExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportAnimeGaleryExcelQuery request, CancellationToken cancellationToken)
    {
        var galeries = (await repository.GetAllAnimeGaleries()).ToList();
        var mediaRows = new List<GaleryMediaExcelRow>();

        foreach (var galery in galeries)
        {
            var media = await mediaRepository.GetMediaByRefId(galery.Id, MediaType.AnimeGalery);
            mediaRows.AddRange(media
                .OrderBy(m => m.OrderIndex)
                .Select(m => new GaleryMediaExcelRow
                {
                    GaleryId = galery.Id,
                    Url = m.Url,
                    OrderIndex = m.OrderIndex,
                }));
        }

        using var stream = ExcelHelper.CreateAnimeGaleryExcel(galeries, mediaRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"anime-galery-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
