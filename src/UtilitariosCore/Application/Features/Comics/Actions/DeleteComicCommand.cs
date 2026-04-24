using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public record DeleteComicCommand(int Id) : IRequest<Result>
{
    internal sealed class Handler(IComicRepository repository)
        : IRequestHandler<DeleteComicCommand, Result>
    {
        public async Task<Result> Handle(DeleteComicCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetComicById(request.Id);
            if (item is null) return Errors.NotFound();
            await repository.DeleteComic(request.Id);
            return Results.NoContent();
        }
    }
}
