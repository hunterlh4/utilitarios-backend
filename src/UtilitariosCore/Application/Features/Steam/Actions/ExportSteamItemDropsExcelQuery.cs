using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItemDrops.Actions;

public record ExportSteamItemDropsExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportSteamItemDropsExcelQueryHandler(ISteamRepository repository)
    : IRequestHandler<ExportSteamItemDropsExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportSteamItemDropsExcelQuery request, CancellationToken cancellationToken)
    {
        var drops = await repository.GetAllDrops();

        var rows = drops.Select(drop => new SteamItemDrop
        {
            Id = drop.Id,
            SteamItemId = drop.SteamItemId,
            Quantity = drop.Quantity,
            Price = drop.Price,
            SalePrice = drop.SalePrice,
            Total = drop.Total,
            CreatedAt = drop.CreatedAt,
        }).ToList();

        using var stream = ExcelHelper.CreateSteamItemDropsExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"steam-drop.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
