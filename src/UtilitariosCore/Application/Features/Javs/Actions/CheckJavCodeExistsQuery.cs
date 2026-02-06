using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record CheckJavCodeExistsQuery(string Code) : IRequest<Result<bool>>;

internal sealed class CheckJavCodeExistsQueryHandler(IJavRepository javRepository) 
    : IRequestHandler<CheckJavCodeExistsQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckJavCodeExistsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Errors.BadRequest("Code is required.");
        }

        var existingJav = await javRepository.GetJavByCode(request.Code);
        return Results.Created(existingJav != null);
    }
}
