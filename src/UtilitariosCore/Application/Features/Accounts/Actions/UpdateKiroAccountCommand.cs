using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateKiroAccountCommand : IRequest<Result>
{
    public int Id { get; set; }
    public LinkedAccountType LinkedType { get; set; }
    public int RefId { get; set; }
    public bool IsNew { get; set; }
    public DateTime? LastUsed { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateKiroAccountCommand, Result>
    {
        public async Task<Result> Handle(UpdateKiroAccountCommand r, CancellationToken ct)
        {
            await repo.UpdateKiro(new AccountKiro
            {
                Id = r.Id, LinkedType = r.LinkedType, RefId = r.RefId,
                IsNew = r.IsNew, LastUsed = r.LastUsed
            });
            return Results.NoContent();
        }
    }
}
