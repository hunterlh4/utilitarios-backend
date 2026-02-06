using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record UpdateHentaiCommand(int Id) : IRequest<Result>
{
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public Domain.Enums.ContentStatus Status { get; set; }

    internal sealed class Handler(IHentaiRepository hentaiRepository) 
        : IRequestHandler<UpdateHentaiCommand, Result>
    {
        public async Task<Result> Handle(UpdateHentaiCommand request, CancellationToken cancellationToken)
        {
            var item = await hentaiRepository.GetHentaiById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            item.Title = request.Title;
            item.Image = request.Image;
            item.Episodes = request.Episodes;
            item.Status = request.Status;

            await hentaiRepository.UpdateHentai(item);
            return Results.NoContent();
        }
    }
}
