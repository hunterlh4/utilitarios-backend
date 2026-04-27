using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record ExportSteamItemsExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportSteamItemsExcelQueryHandler(ISteamRepository repository)
    : IRequestHandler<ExportSteamItemsExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportSteamItemsExcelQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllItems();

        var rows = items.Select(item => new SteamItem
        {
            ExternalId = item.ExternalId,
            Name = item.Name,
            Image = item.Image,
            Price = item.Price,
            Game = (GameType)item.Game,
            MarketUrl = item.MarketUrl,
        }).ToList();

        using var stream = ExcelHelper.CreateSteamItemsExcel(rows);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"steam-item.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
