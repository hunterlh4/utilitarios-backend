using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Media.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Media.Actions;

public record UploadImageCommand(byte[] ImageData, string FileName, MediaType Type, int RefId)
    : IRequest<Result<UploadImageDto>>
{
    public sealed class Validator : AbstractValidator<UploadImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ImageData).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("El nombre de archivo es requerido.");
            RuleFor(x => x.Type).IsInEnum().WithMessage("Tipo de media inválido.");
            RuleFor(x => x.RefId).GreaterThan(0).WithMessage("El RefId debe ser mayor a 0.");
        }
    }

    internal sealed class Handler(IImgBBService imgBBService, IMediaRepository mediaRepository)
        : IRequestHandler<UploadImageCommand, Result<UploadImageDto>>
    {
        public async Task<Result<UploadImageDto>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(request.ImageData);
            var uploadResult = await imgBBService.UploadImageAsync(stream, request.FileName);

            if (uploadResult == null) return Errors.BadRequest("Error al subir la imagen a ImgBB.");

            var existingMedia = await mediaRepository.GetMediaByRefId(request.RefId, request.Type);
            var nextOrderIndex = existingMedia.Any() ? existingMedia.Max(m => m.OrderIndex) + 1 : 1;

            var media = new Domain.Models.Media
            {
                Type = request.Type,
                RefId = request.RefId,
                Url = uploadResult.Url,
                Thumbnail = uploadResult.Thumbnail,
                DeleteUrl = uploadResult.DeleteUrl,
                OrderIndex = nextOrderIndex,
                CreatedAt = DateTime.UtcNow
            };

            var mediaId = await mediaRepository.CreateMedia(media);

            return Results.Created(new UploadImageDto
            {
                Id = mediaId,
                Url = uploadResult.Url,
                Thumbnail = uploadResult.Thumbnail,
                DeleteUrl = uploadResult.DeleteUrl,
                OrderIndex = nextOrderIndex
            });
        }
    }
}
