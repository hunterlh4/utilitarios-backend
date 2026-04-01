using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateEmailAccountCommand : IRequest<Result<int>>
{
    public string Provider { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? RecoveryEmailId { get; set; }

    public sealed class Validator : AbstractValidator<CreateEmailAccountCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Provider).NotEmpty().MaximumLength(50);
        }
    }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<CreateEmailAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateEmailAccountCommand r, CancellationToken ct)
        {
            var id = await repo.CreateEmail(new AccountEmail
            {
                Provider = r.Provider, Email = r.Email, Password = r.Password,
                Phone = r.Phone, RecoveryEmailId = r.RecoveryEmailId, CreatedAt = DateTime.Now
            });
            return Results.Created(id);
        }
    }
}
