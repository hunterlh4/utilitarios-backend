using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Dtos;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record ImportAccountExcelCommand : IRequest<Result<ImportExcelResult>>
{
    public byte[] FileBytes { get; init; } = [];
}

internal sealed class ImportAccountExcelCommandHandler(IAccountRepository repository)
    : IRequestHandler<ImportAccountExcelCommand, Result<ImportExcelResult>>
{
    public async Task<Result<ImportExcelResult>> Handle(ImportAccountExcelCommand request, CancellationToken cancellationToken)
    {
        if (request.FileBytes.Length == 0)
            return Errors.BadRequest("Archivo Excel vacío.");

        using var stream = new MemoryStream(request.FileBytes);
        var data = ExcelHelper.ReadAccountExcel(stream);

        int created = 0, updated = 0, skipped = 0, invalid = 0;

        // Emails
        var existingEmails = (await repository.GetEmails()).ToDictionary(e => e.Id);
        foreach (var row in data.Emails)
        {
            if (string.IsNullOrWhiteSpace(row.Email)) { invalid++; continue; }
            if (row.Id > 0 && existingEmails.TryGetValue(row.Id, out var existing))
            {
                var hasChanges = existing.Provider != row.Provider || existing.Email != row.Email ||
                    existing.Password != row.Password || existing.Phone != row.Phone ||
                    existing.RecoveryEmailId != row.RecoveryEmailId;
                if (!hasChanges) { skipped++; continue; }
                await repository.UpdateEmail(new AccountEmail { Id = row.Id, Provider = row.Provider, Email = row.Email, Password = row.Password, Phone = row.Phone, RecoveryEmailId = row.RecoveryEmailId });
                updated++;
            }
            else
            {
                await repository.CreateEmail(new AccountEmail { Provider = row.Provider, Email = row.Email, Password = row.Password, Phone = row.Phone, RecoveryEmailId = row.RecoveryEmailId, CreatedAt = DateTime.UtcNow });
                created++;
            }
        }

        // Steams
        var existingSteams = (await repository.GetSteams()).ToDictionary(s => s.Id);
        foreach (var row in data.Steams)
        {
            if (string.IsNullOrWhiteSpace(row.Username)) { invalid++; continue; }
            if (row.Id > 0 && existingSteams.TryGetValue(row.Id, out var existing))
            {
                await repository.UpdateSteam(new AccountSteam { Id = row.Id, EmailId = row.EmailId, Username = row.Username, Password = row.Password, Phone = row.Phone, ProfileUrl = row.ProfileUrl, ImageUrl = row.ImageUrl, HasDota2 = row.HasDota2, HasCS2 = row.HasCS2, IsUnlimited = row.IsUnlimited, IsVacBanned = row.IsVacBanned, HasSteamMobile = row.HasSteamMobile, LastPurchaseDate = row.LastPurchaseDate });
                updated++;
            }
            else
            {
                await repository.CreateSteam(new AccountSteam { EmailId = row.EmailId, Username = row.Username, Password = row.Password, Phone = row.Phone, ProfileUrl = row.ProfileUrl, ImageUrl = row.ImageUrl, HasDota2 = row.HasDota2, HasCS2 = row.HasCS2, IsUnlimited = row.IsUnlimited, IsVacBanned = row.IsVacBanned, HasSteamMobile = row.HasSteamMobile, LastPurchaseDate = row.LastPurchaseDate, CreatedAt = DateTime.UtcNow });
                created++;
            }
        }

        // GitHubs
        var existingGitHubs = (await repository.GetGitHubs()).ToDictionary(g => g.Id);
        foreach (var row in data.GitHubs)
        {
            if (string.IsNullOrWhiteSpace(row.Username)) { invalid++; continue; }
            if (row.Id > 0 && existingGitHubs.TryGetValue(row.Id, out _))
            {
                await repository.UpdateGitHub(new AccountGitHub { Id = row.Id, EmailId = row.EmailId, Username = row.Username, Password = row.Password, ProfileUrl = row.ProfileUrl });
                updated++;
            }
            else
            {
                await repository.CreateGitHub(new AccountGitHub { EmailId = row.EmailId, Username = row.Username, Password = row.Password, ProfileUrl = row.ProfileUrl, CreatedAt = DateTime.UtcNow });
                created++;
            }
        }

        // Generals
        var existingGenerals = (await repository.GetGenerals()).ToDictionary(g => g.Id);
        foreach (var row in data.Generals)
        {
            if (string.IsNullOrWhiteSpace(row.Username)) { invalid++; continue; }
            var platform = Enum.IsDefined(typeof(GeneralPlatform), row.Platform) ? (GeneralPlatform)row.Platform : GeneralPlatform.Other;
            if (row.Id > 0 && existingGenerals.TryGetValue(row.Id, out _))
            {
                await repository.UpdateGeneral(new AccountGeneral { Id = row.Id, Platform = platform, Username = row.Username, Password = row.Password, EmailId = row.EmailId, ProfileUrl = row.ProfileUrl });
                updated++;
            }
            else
            {
                await repository.CreateGeneral(new AccountGeneral { Platform = platform, Username = row.Username, Password = row.Password, EmailId = row.EmailId, ProfileUrl = row.ProfileUrl, CreatedAt = DateTime.UtcNow });
                created++;
            }
        }

        // Kiros
        var existingKiros = (await repository.GetKiro()).ToDictionary(k => k.Id);
        foreach (var row in data.Kiros)
        {
            if (row.RefId <= 0) { invalid++; continue; }
            var linkedType = Enum.IsDefined(typeof(LinkedAccountType), row.LinkedType) ? (LinkedAccountType)row.LinkedType : LinkedAccountType.Email;
            if (row.Id > 0 && existingKiros.TryGetValue(row.Id, out _))
            {
                await repository.UpdateKiro(new AccountKiro { Id = row.Id, LinkedType = linkedType, RefId = row.RefId, IsNew = row.IsNew, LastUsed = row.LastUsed });
                updated++;
            }
            else
            {
                await repository.CreateKiro(new AccountKiro { LinkedType = linkedType, RefId = row.RefId, IsNew = row.IsNew, LastUsed = row.LastUsed, CreatedAt = DateTime.UtcNow });
                created++;
            }
        }

        return new ImportExcelResult { Created = created, Updated = updated, Skipped = skipped, Invalid = invalid };
    }
}
