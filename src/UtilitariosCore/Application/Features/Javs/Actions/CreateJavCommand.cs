using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Javs.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public class CreateJavCommand : IRequest<Result<CreateJavDto>>
{
    public string Code { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = new();
    public string Image { get; set; } = string.Empty;
    public List<string> Links { get; set; } = new();

    public sealed class Validator : AbstractValidator<CreateJavCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("El código es requerido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.ActressIds).NotEmpty().WithMessage("Se requiere al menos una actriz.");
            RuleForEach(x => x.ActressIds).GreaterThan(0).WithMessage("El ID de la actriz debe ser mayor a 0.");
            RuleForEach(x => x.Links).NotEmpty().WithMessage("El link no puede estar vacío.");
        }
    }

    internal sealed class Handler(
        IJavRepository javRepository,
        ILinkRepository linkRepository)
        : IRequestHandler<CreateJavCommand, Result<CreateJavDto>>
    {
        public async Task<Result<CreateJavDto>> Handle(CreateJavCommand request, CancellationToken cancellationToken)
        {
            var newJav = new Jav
            {
                Code = request.Code.ToUpper(),
                Image = request.Image,
                Status = ContentStatus.Proximamente,
                CreatedAt = DateTime.UtcNow
            };

            var javId = await javRepository.CreateJav(newJav);

            foreach (var actressId in request.ActressIds)
                await javRepository.AddActressToJav(javId, actressId);

            if (request.Links != null && request.Links.Count > 0)
            {
                foreach (var url in request.Links)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        await linkRepository.CreateLink(new Link
                        {
                            Type = LinkType.Jav,
                            RefId = javId,
                            Name = null,
                            Url = url,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            return Results.Created(new CreateJavDto { Id = javId });
        }
    }
}
