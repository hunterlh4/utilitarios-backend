using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record UpdateSteamLastPlayCommand : IRequest<Result>
{
    public int Id { get; set; }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<UpdateSteamLastPlayCommand, Result>
    {
        public async Task<Result> Handle(UpdateSteamLastPlayCommand request, CancellationToken ct)
        {
            await repo.UpdateSteamLastPlay(request.Id, DateTime.Now);
            return Results.NoContent();
        }
    }
}
