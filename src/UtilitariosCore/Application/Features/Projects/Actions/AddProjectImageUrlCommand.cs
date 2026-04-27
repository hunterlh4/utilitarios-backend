using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Projects.Actions;

public class AddProjectImageUrlCommand : IRequest<Result<int>>
{
    public int ProjectId { get; set; }
    public string Url { get; set; } = string.Empty;

    public sealed class Validator : AbstractValidator<AddProjectImageUrlCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId).GreaterThan(0);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(1000);
        }
    }

    internal sealed class Handler(IProjectRepository projectRepository, IMediaRepository mediaRepository)
        : IRequestHandler<AddProjectImageUrlCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AddProjectImageUrlCommand request, CancellationToken cancellationToken)
        {
            var exists = await projectRepository.Exists(request.ProjectId);
            if (!exists) return Errors.NotFound("Proyecto no encontrado.");

            var existing = await mediaRepository.GetMediaByRefId(request.ProjectId, MediaType.Project);
            var nextOrder = existing.Any() ? existing.Max(m => m.OrderIndex) + 1 : 1;

            var media = new Domain.Models.Media
            {
                Type = MediaType.Project,
                RefId = request.ProjectId,
                Url = request.Url,
                OrderIndex = nextOrder,
                CreatedAt = DateTime.UtcNow
            };

            var id = await mediaRepository.CreateMedia(media);
            return Results.Created(id);
        }
    }
}
