using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record ExportJavExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportJavExcelQueryHandler(
    IJavRepository javRepository,
    IActressJavRepository actressRepository,
    ILinkRepository linkRepository,
    ITagRepository tagRepository)
    : IRequestHandler<ExportJavExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportJavExcelQuery request, CancellationToken cancellationToken)
    {
        var javs = (await javRepository.GetAllJavs()).OrderBy(j => j.Id).ToList();
        var actresses = (await actressRepository.GetAllActressJav()).OrderBy(a => a.Id).ToList();
        var actressById = actresses.ToDictionary(a => a.Id);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();

        // ── Hoja 1: Javs ──────────────────────────────────────────────────────
        var ws1 = package.Workbook.Worksheets.Add("Javs");
        SetHeader(ws1, new[] { "Code", "Image", "Status", "Tags" });
        int row = 2;
        foreach (var jav in javs)
        {
            var tags = await tagRepository.GetTagsByRefId(jav.Id, TagType.Jav);
            var tagNames = string.Join(", ", tags.Select(t => t.Name));
            ws1.Cells[row, 1].Value = jav.Code;
            ws1.Cells[row, 2].Value = jav.Image;
            ws1.Cells[row, 3].Value = (int)jav.Status;
            ws1.Cells[row, 4].Value = tagNames;
            row++;
        }
        ws1.Cells[ws1.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 2: ActressJav ────────────────────────────────────────────────
        var ws2 = package.Workbook.Worksheets.Add("ActressJav");
        SetHeader(ws2, new[] { "Name", "Image", "Tags" });
        row = 2;
        foreach (var actress in actresses)
        {
            var tags = await tagRepository.GetTagsByRefId(actress.Id, TagType.ActressJav);
            var tagNames = string.Join(", ", tags.Select(t => t.Name));
            ws2.Cells[row, 1].Value = actress.Name;
            ws2.Cells[row, 2].Value = actress.Image;
            ws2.Cells[row, 3].Value = tagNames;
            row++;
        }
        ws2.Cells[ws2.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 3: JavLinks ──────────────────────────────────────────────────
        var ws3 = package.Workbook.Worksheets.Add("JavLinks");
        SetHeader(ws3, new[] { "Code", "Link" });
        row = 2;
        foreach (var jav in javs)
        {
            var links = await linkRepository.GetLinksByRefId(jav.Id, LinkType.Jav);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
            {
                ws3.Cells[row, 1].Value = jav.Code;
                ws3.Cells[row, 2].Value = link.Url;
                row++;
            }
        }
        ws3.Cells[ws3.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 4: ActressJavLinks ───────────────────────────────────────────
        var ws4 = package.Workbook.Worksheets.Add("ActressJavLinks");
        SetHeader(ws4, new[] { "ActressName", "Link" });
        row = 2;
        foreach (var actress in actresses)
        {
            var links = await linkRepository.GetLinksByRefId(actress.Id, LinkType.ActressJav);
            foreach (var link in links.OrderBy(l => l.OrderIndex ?? int.MaxValue))
            {
                ws4.Cells[row, 1].Value = actress.Name;
                ws4.Cells[row, 2].Value = link.Url;
                row++;
            }
        }
        ws4.Cells[ws4.Dimension?.Address ?? "A1"].AutoFitColumns();

        // ── Hoja 5: Relations (Code ↔ ActressName) ────────────────────────────
        var ws5 = package.Workbook.Worksheets.Add("Relations");
        SetHeader(ws5, new[] { "Code", "ActressName" });
        row = 2;
        foreach (var jav in javs)
        {
            var actressIds = await javRepository.GetActressIdsByJavId(jav.Id);
            foreach (var actressId in actressIds)
            {
                if (actressById.TryGetValue(actressId, out var actress))
                {
                    ws5.Cells[row, 1].Value = jav.Code;
                    ws5.Cells[row, 2].Value = actress.Name;
                    row++;
                }
            }
        }
        ws5.Cells[ws5.Dimension?.Address ?? "A1"].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return new ExcelFileDto
        {
            FileName = $"jav-export.xlsx",
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
