using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record ExportGirlGaleryExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportGirlGaleryExcelQueryHandler(
    IGaleryRepository repository,
    IMediaRepository mediaRepository)
    : IRequestHandler<ExportGirlGaleryExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportGirlGaleryExcelQuery request, CancellationToken cancellationToken)
    {
        var galeries = (await repository.GetAllGirlGaleries()).ToList();
        var mediaRows = new List<GaleryMediaExcelRow>();

        foreach (var galery in galeries)
        {
            var media = await mediaRepository.GetMediaByRefId(galery.Id, MediaType.GirlGalery);
            mediaRows.AddRange(media
                .OrderBy(m => m.OrderIndex)
                .Select(m => new GaleryMediaExcelRow
                {
                    GaleryId = galery.Id,
                    Url = m.Url,
                    OrderIndex = m.OrderIndex,
                }));
        }

        using var stream = ExcelHelper.CreateGirlGaleryExcel(galeries, mediaRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"girl-galery-{DateTime.Now:dd-MM-yyyy}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
