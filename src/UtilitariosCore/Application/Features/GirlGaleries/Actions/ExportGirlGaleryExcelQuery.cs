using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record ExportGirlGaleryExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportGirlGaleryExcelQueryHandler(
    IGaleryRepository repository,
    IMediaRepository mediaRepository,
    ILinkRepository linkRepository)
    : IRequestHandler<ExportGirlGaleryExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportGirlGaleryExcelQuery request, CancellationToken cancellationToken)
    {
        var galeries = (await repository.GetAllGirlGaleries()).ToList();
        var mediaRows = new List<GaleryMediaExcelRow>();
        var linkRows = new List<GaleryLinkExcelRow>();

        foreach (var galery in galeries)
        {
            var media = await mediaRepository.GetMediaByRefId(galery.Id, MediaType.GirlGalery);
            var links = await linkRepository.GetLinksByRefId(galery.Id, LinkType.GirlGalery);
            mediaRows.AddRange(media
                .OrderBy(m => m.OrderIndex)
                .Select(m => new GaleryMediaExcelRow
                {
                    GaleryId = galery.Id,
                    Url = m.Url,
                    OrderIndex = m.OrderIndex,
                }));

            linkRows.AddRange(links
                .OrderBy(l => l.OrderIndex)
                .Select(l => new GaleryLinkExcelRow
                {
                    GaleryId = galery.Id,
                    Name = l.Name,
                    Url = l.Url,
                    OrderIndex = l.OrderIndex ?? 0,
                }));
        }

        using var stream = ExcelHelper.CreateGirlGaleryExcel(galeries, mediaRows, linkRows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"girl-galery.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
