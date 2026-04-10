using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record ExportActressAdultExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportActressAdultExcelQueryHandler(IActressAdultRepository repository)
    : IRequestHandler<ExportActressAdultExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressAdultExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressAdults()).ToList();

        using var stream = ExcelHelper.CreateActressAdultExcel(actresses);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"actress-adult-{DateTime.Now:yyyyMMddHHmmss}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
