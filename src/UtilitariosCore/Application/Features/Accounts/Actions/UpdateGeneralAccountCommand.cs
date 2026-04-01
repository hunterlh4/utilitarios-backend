using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateGeneralAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
    public GeneralPlatform Platform { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? EmailId { get; set; }
    public string? ProfileUrl { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateGeneralAccountCommand, Result>
    {
        public async Task<Result> Handle(UpdateGeneralAccountCommand r, CancellationToken ct)
        {
            await repo.UpdateGeneral(new AccountGeneral
            {
                Id = r.Id, Platform = r.Platform, Username = r.Username, Password = r.Password,
                EmailId = r.EmailId, ProfileUrl = r.ProfileUrl
            });
            return Results.NoContent();
        }
    }
}
