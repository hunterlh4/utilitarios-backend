using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateEmailAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? RecoveryEmailId { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateEmailAccountCommand, Result>
    {
        public async Task<Result> Handle(UpdateEmailAccountCommand r, CancellationToken ct)
        {
            await repo.UpdateEmail(new AccountEmail
            {
                Id = r.Id, Provider = r.Provider, Email = r.Email, Password = r.Password,
                Phone = r.Phone, RecoveryEmailId = r.RecoveryEmailId
            });
            return Results.NoContent();
        }
    }
}
