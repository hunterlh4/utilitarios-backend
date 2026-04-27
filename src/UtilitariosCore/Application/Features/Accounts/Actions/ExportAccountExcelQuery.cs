using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record ExportAccountExcelQuery : IRequest<Result<ExcelFileDto>>;

internal sealed class ExportAccountExcelQueryHandler(IAccountRepository repository)
    : IRequestHandler<ExportAccountExcelQuery, Result<ExcelFileDto>>
{
    public async Task<Result<ExcelFileDto>> Handle(ExportAccountExcelQuery request, CancellationToken cancellationToken)
    {
        var emails   = (await repository.GetEmails()).ToList();
        var steams   = (await repository.GetSteams()).ToList();
        var gitHubs  = (await repository.GetGitHubs()).ToList();
        var generals = (await repository.GetGenerals()).ToList();
        var kiros    = (await repository.GetKiro()).ToList();

        var data = new AccountExcelData
        {
            Emails = emails.Select(e => new AccountEmailExcelRow
            {
                Id = e.Id, Provider = e.Provider, Email = e.Email,
                Password = e.Password, Phone = e.Phone, RecoveryEmailId = e.RecoveryEmailId,
            }).ToList(),
            Steams = steams.Select(s => new AccountSteamExcelRow
            {
                Id = s.Id, EmailId = s.EmailId, Username = s.Username,
                Password = s.Password, Phone = s.Phone, ProfileUrl = s.ProfileUrl,
                HasDota2 = s.HasDota2, HasCS2 = s.HasCS2, IsUnlimited = s.IsUnlimited,
                IsVacBanned = s.IsVacBanned, HasSteamMobile = s.HasSteamMobile,
            }).ToList(),
            GitHubs = gitHubs.Select(g => new AccountGitHubExcelRow
            {
                Id = g.Id, EmailId = g.EmailId, Username = g.Username,
                Password = g.Password, ProfileUrl = g.ProfileUrl,
            }).ToList(),
            Generals = generals.Select(g => new AccountGeneralExcelRow
            {
                Id = g.Id, Platform = (int)g.Platform, Username = g.Username,
                Password = g.Password, EmailId = g.EmailId, ProfileUrl = g.ProfileUrl,
            }).ToList(),
            Kiros = kiros.Select(k => new AccountKiroExcelRow
            {
                Id = k.Id, LinkedType = (int)k.LinkedType, RefId = k.RefId,
                IsNew = k.IsNew, LastUsed = k.LastUsed,
            }).ToList(),
        };

        using var stream = ExcelHelper.CreateAccountExcel(data);
        var base64 = Convert.ToBase64String(stream.ToArray());

        return new ExcelFileDto
        {
            FileName = $"accounts.xlsx",
            Base64 = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}"
        };
    }
}
