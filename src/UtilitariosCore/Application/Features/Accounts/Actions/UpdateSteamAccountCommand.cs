using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateSteamAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfileUrl { get; set; }
    public bool HasDota2 { get; set; }
    public bool HasCS2 { get; set; }
    public bool IsUnlimited { get; set; }
    public bool IsVacBanned { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateSteamAccountCommand, Result>
    {
        public async Task<Result> Handle(UpdateSteamAccountCommand r, CancellationToken ct)
        {
            await repo.UpdateSteam(new AccountSteam
            {
                Id = r.Id, EmailId = r.EmailId, Username = r.Username, Password = r.Password,
                Phone = r.Phone, ProfileUrl = r.ProfileUrl,
                HasDota2 = r.HasDota2, HasCS2 = r.HasCS2, IsUnlimited = r.IsUnlimited, IsVacBanned = r.IsVacBanned
            });
            return Results.NoContent();
        }
    }
}
