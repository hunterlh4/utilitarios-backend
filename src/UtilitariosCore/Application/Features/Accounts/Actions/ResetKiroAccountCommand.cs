using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record ResetKiroAccountCommand : IRequest<Result<int>>
{
    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<ResetKiroAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(ResetKiroAccountCommand r, CancellationToken ct)
        {
            // Hora Perú (UTC-5)
            var peruNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");
            // Día 1 del mes actual en hora Perú
            var threshold = new DateTime(peruNow.Year, peruNow.Month, 1, 0, 0, 0);

            int count = await repo.ResetKiro(threshold);
            return count;
        }
    }
}
