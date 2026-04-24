using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public class CreateComicCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<CreateComicCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es requerido.");
        }
    }

    internal sealed class Handler(IComicRepository repository)
        : IRequestHandler<CreateComicCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateComicCommand request, CancellationToken cancellationToken)
        {
            var id = await repository.CreateComic(new Comic
            {
                Name = request.Name.Trim(),
                Image = request.Image.Trim(),
                Url = request.Url.Trim(),
                Category = request.Category.Trim(),
                CreatedAt = DateTime.UtcNow
            });
            return Results.Created(id);
        }
    }
}
