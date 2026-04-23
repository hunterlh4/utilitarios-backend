using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Javs.Actions;

public record UploadJavImageCommand : IRequest<Result>
{
    public int JavId { get; init; }
    public byte[] ImageData { get; init; } = [];
    public string FileName { get; init; } = string.Empty;

    public sealed class Validator : AbstractValidator<UploadJavImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.JavId).GreaterThan(0);
            RuleFor(x => x.ImageData).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}

internal sealed class UploadJavImageCommandHandler(
    IJavRepository javRepository,
    IImgBBService imgBBService)
    : IRequestHandler<UploadJavImageCommand, Result>
{
    public async Task<Result> Handle(UploadJavImageCommand request, CancellationToken cancellationToken)
    {
        var jav = await javRepository.GetJavById(request.JavId);
        if (jav is null) return Errors.NotFound("JAV no encontrado.");

        using var stream = new MemoryStream(request.ImageData);
        var fileName = Path.GetFileNameWithoutExtension(request.FileName) + ".webp";
        var uploadResult = await imgBBService.UploadImageAsync(stream, fileName);

        if (uploadResult is null) return Errors.BadRequest("Error al subir la imagen a ImgBB.");

        await javRepository.UpdateJavImage(request.JavId, uploadResult.Url);
        return Results.NoContent();
    }
}
