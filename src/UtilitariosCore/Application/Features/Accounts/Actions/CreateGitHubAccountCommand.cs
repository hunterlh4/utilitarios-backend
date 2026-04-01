using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateGitHubAccountCommand : IRequest<Result<int>>
{
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ProfileUrl { get; set; }

    public sealed class Validator : AbstractValidator<CreateGitHubAccountCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<CreateGitHubAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateGitHubAccountCommand r, CancellationToken ct)
        {
            var id = await repo.CreateGitHub(new AccountGitHub
            {
                EmailId = r.EmailId, Username = r.Username, Password = r.Password,
                ProfileUrl = r.ProfileUrl, CreatedAt = DateTime.Now
            });
            return Results.Created(id);
        }
    }
}
