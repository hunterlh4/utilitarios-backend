using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public class UpdateActressCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    internal sealed class Handler(IActressRepository actressRepository) 
        : IRequestHandler<UpdateActressCommand, Result>
    {
        public async Task<Result> Handle(UpdateActressCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Name is required.");
            }

            var actress = await actressRepository.GetActressById(request.Id);
            
            if (actress == null)
            {
                return Errors.NotFound("Actress not found.");
            }

            actress.Name = request.Name;
            await actressRepository.UpdateActress(actress);

            return Results.NoContent();
        }
    }
}
