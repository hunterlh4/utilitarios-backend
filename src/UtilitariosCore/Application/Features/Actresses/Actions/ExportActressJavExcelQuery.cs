using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record ExportActressJavExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportActressJavExcelQueryHandler(IActressJavRepository repository)
    : IRequestHandler<ExportActressJavExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressJavExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressJav()).ToList();

        using var stream = ExcelHelper.CreateActressJavExcel(actresses);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"actress-jav-{DateTime.Now:yyyyMMddHHmmss}.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
