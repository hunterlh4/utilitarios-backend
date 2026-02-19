using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.ActressAdults.Actions;

public class UpdateActressAdultCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IActressAdultRepository actressAdultRepository) 
        : IRequestHandler<UpdateActressAdultCommand, Result>
    {
        public async Task<Result> Handle(UpdateActressAdultCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var actress = await actressAdultRepository.GetActressAdultById(request.Id);
            
            if (actress == null)
            {
                return Errors.NotFound("Actress not found.");
            }

            actress.Name = request.Name;
            await actressAdultRepository.UpdateActressAdult(actress);

            return Results.NoContent();
        }
    }
}
