using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public record ExportActressAdultExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportActressAdultExcelQueryHandler(
    IActressAdultRepository repository,
    IVideoAdultRepository videoAdultRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<ExportActressAdultExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportActressAdultExcelQuery request, CancellationToken cancellationToken)
    {
        var actresses = (await repository.GetAllActressAdults()).OrderBy(a => a.Id).ToList();
        var videos = (await videoAdultRepository.GetAllVideoAdults()).OrderBy(v => v.Id).ToList();
        var actressById = actresses.ToDictionary(a => a.Id);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();

        // ── Hoja 1: ActressAdult ────────────────────────────────────────────────
        var ws1 = package.Workbook.Worksheets.Add("ActressAdult");
        SetHeader(ws1, new[] { "Id", "Name", "Image", "TagIds" });
        int row = 2;
        foreach (var actress in actresses)
        {
            var tags = await tagRepository.GetTagsByRefId(actress.Id, TagType.ActressAdult);
            var tagIds = tags.Select(t => t.Id).Where(id => id > 0).OrderBy(id => id).ToList();
            var tagIdsString = tagIds.Count > 0 ? string.Join(",", tagIds) : null;
            
            ws1.Cells[row, 1].Value = actress.Id;
            ws1.Cells[row, 2].Value = actress.Name;
            ws1.Cells[row, 3].Value = actress.Image;
            ws1.Cells[row, 4].Value = tagIdsString;
            row++;
        }
        ws1.Cells[ws1.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 2: Videos ──────────────────────────────────────────────────────
        var ws2 = package.Workbook.Worksheets.Add("Videos");
        SetHeader(ws2, new[] { "Id", "Source", "ExternalId", "VideoUrl", "Title", "ThumbnailUrl", "Status", "TagIds" });
        row = 2;
        foreach (var video in videos)
        {
            var tags = await tagRepository.GetTagsByRefId(video.Id, TagType.VideoAdult);
            var tagIds = tags.Select(t => t.Id).Where(id => id > 0).OrderBy(id => id).ToList();
            var tagIdsString = tagIds.Count > 0 ? string.Join(",", tagIds) : null;
            
            ws2.Cells[row, 1].Value = video.Id;
            ws2.Cells[row, 2].Value = video.Source;
            ws2.Cells[row, 3].Value = video.ExternalId;
            ws2.Cells[row, 4].Value = video.VideoUrl;
            ws2.Cells[row, 5].Value = video.Title;
            ws2.Cells[row, 6].Value = video.ThumbnailUrl;
            ws2.Cells[row, 7].Value = (int)video.Status;
            ws2.Cells[row, 8].Value = tagIdsString;
            row++;
        }
        ws2.Cells[ws2.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 3: ActressAdultLinks ───────────────────────────────────────────
        var ws3 = package.Workbook.Worksheets.Add("ActressAdultLinks");
        SetHeader(ws3, new[] { "ActressId", "ActressName", "Link", "OrderIndex" });
        row = 2;
        foreach (var actress in actresses)
        {
            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressAdult);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
            {
                ws3.Cells[row, 1].Value = actress.Id;
                ws3.Cells[row, 2].Value = actress.Name;
                ws3.Cells[row, 3].Value = link.Url;
                ws3.Cells[row, 4].Value = link.OrderIndex;
                row++;
            }
        }
        ws3.Cells[ws3.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 4: Relations (VideoId ↔ ActressId) ────────────────────────────
        var ws4 = package.Workbook.Worksheets.Add("Relations");
        SetHeader(ws4, new[] { "VideoId", "ActressId", "ExternalId", "ActressName" });
        row = 2;
        foreach (var video in videos)
        {
            var actressIds = await videoAdultRepository.GetActressIdsByVideoId(video.Id);
            foreach (var actressId in actressIds)
            {
                if (actressById.TryGetValue(actressId, out var actress))
                {
                    ws4.Cells[row, 1].Value = video.Id;
                    ws4.Cells[row, 2].Value = actress.Id;
                    ws4.Cells[row, 3].Value = video.ExternalId;
                    ws4.Cells[row, 4].Value = actress.Name;
                    row++;
                }
            }
        }
        ws4.Cells[ws4.Dimension?.Address ?? "A1"].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return new ExcelFileDto
        {
            FileName = $"actress-adult-export.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{Convert.ToBase64String(stream.ToArray())}"
        };
    }

    private static void SetHeader(ExcelWorksheet ws, string[] headers)
    {
        for (int i = 0; i < headers.Length; i++)
            ws.Cells[1, i + 1].Value = headers[i];

        var range = ws.Cells[1, 1, 1, headers.Length];
        range.Style.Font.Bold = true;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }
}
