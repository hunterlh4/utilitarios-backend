using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record DeleteHentaiCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IHentaiRepository hentaiRepository) 
        : IRequestHandler<DeleteHentaiCommand, Result>
    {
        public async Task<Result> Handle(DeleteHentaiCommand request, CancellationToken cancellationToken)
        {
            var item = await hentaiRepository.GetHentaiById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            await hentaiRepository.DeleteHentai(request.Id);
            return Results.NoContent();
        }
    }
}
