using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateGitHubAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfileUrl { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateGitHubAccountCommand, Result>
    {
        public async Task<Result> Handle(UpdateGitHubAccountCommand r, CancellationToken ct)
        {
            await repo.UpdateGitHub(new AccountGitHub
            {
                Id = r.Id, EmailId = r.EmailId, Username = r.Username,
                Password = r.Password, ProfileUrl = r.ProfileUrl
            });
            return Results.NoContent();
        }
    }
}
