using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItemPurchases.Actions;

public record ExportSteamItemPurchasesExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportSteamItemPurchasesExcelQueryHandler(ISteamRepository repository)
    : IRequestHandler<ExportSteamItemPurchasesExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportSteamItemPurchasesExcelQuery request, CancellationToken cancellationToken)
    {
        var purchases = await repository.GetAllPurchase();

        var rows = purchases.Select(item => new SteamItemPurchase
        {
            Id = item.Id,
            SteamItemId = item.SteamItemId,
            PurchasePrice = item.PurchasePrice,
            SalePrice = item.SalePrice,
            Profit = item.Profit,
            Status = item.Status,
            PurchaseDate = item.PurchaseDate,
            SaleDate = item.SaleDate,
            CreatedAt = item.CreatedAt,
        }).ToList();

        using var stream = ExcelHelper.CreateSteamItemPurchasesExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"steam-purchase.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
