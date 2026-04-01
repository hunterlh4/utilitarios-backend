using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateKiroAccountCommand : IRequest<Result<int>>
{
    public LinkedAccountType LinkedType { get; set; }
    public int RefId { get; set; }
    public bool IsNew { get; set; } = true;
    public DateTime? LastUsed { get; set; }

    public sealed class Validator : AbstractValidator<CreateKiroAccountCommand>
    {
        public Validator()
        {
            RuleFor(x => x.LinkedType).IsInEnum();
            RuleFor(x => x.RefId).GreaterThan(0);
        }
    }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<CreateKiroAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateKiroAccountCommand r, CancellationToken ct)
        {
            var id = await repo.CreateKiro(new AccountKiro
            {
                LinkedType = r.LinkedType, RefId = r.RefId,
                IsNew = r.IsNew, LastUsed = r.LastUsed, CreatedAt = DateTime.Now
            });
            return Results.Created(id);
        }
    }
}
