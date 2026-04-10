using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Actresses.Actions;

public record UploadActressJavImageCommand : IRequest<Result>
{
    public int ActressId { get; init; }
    public byte[] ImageData { get; init; } = [];
    public string FileName { get; init; } = string.Empty;

    public sealed class Validator : AbstractValidator<UploadActressJavImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ActressId).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.ImageData).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("El nombre de archivo es requerido.");
        }
    }
}

internal sealed class UploadActressJavImageCommandHandler(
    IActressJavRepository actressRepository,
    IImgBBService imgBBService)
    : IRequestHandler<UploadActressJavImageCommand, Result>
{
    public async Task<Result> Handle(UploadActressJavImageCommand request, CancellationToken cancellationToken)
    {
        var actress = await actressRepository.GetActressJavById(request.ActressId);
        if (actress is null)
            return Errors.NotFound("Actriz no encontrada.");

        using var stream = new MemoryStream(request.ImageData);
        var fileNameWithWebpExtension = Path.GetFileNameWithoutExtension(request.FileName) + ".webp";
        var uploadResult = await imgBBService.UploadImageAsync(stream, fileNameWithWebpExtension);

        if (uploadResult is null)
            return Errors.BadRequest("Error al subir la imagen a ImgBB.");

        await actressRepository.UpdateActressJavImage(request.ActressId, uploadResult.Url);

        return Results.NoContent();
    }
}
