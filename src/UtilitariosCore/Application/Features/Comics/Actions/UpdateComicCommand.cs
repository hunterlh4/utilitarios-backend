using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public class UpdateComicCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<UpdateComicCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(IComicRepository repository)
        : IRequestHandler<UpdateComicCommand, Result>
    {
        public async Task<Result> Handle(UpdateComicCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetComicById(request.Id);
            if (item is null) return Errors.NotFound("Comic no encontrado.");

            item.Name = request.Name.Trim();
            item.Image = request.Image.Trim();
            item.Url = request.Url.Trim();
            item.Category = request.Category.Trim();

            await repository.UpdateComic(item);
            return Results.NoContent();
        }
    }
}
