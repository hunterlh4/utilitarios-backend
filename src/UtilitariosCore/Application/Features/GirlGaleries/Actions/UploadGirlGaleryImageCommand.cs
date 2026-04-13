using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.GirlGaleries.Actions;

public record UploadGirlGaleryImageCommand : IRequest<Result>
{
    public int GaleryId { get; init; }
    public byte[] ImageData { get; init; } = [];
    public string FileName { get; init; } = string.Empty;

    public sealed class Validator : AbstractValidator<UploadGirlGaleryImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.GaleryId).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.ImageData).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("El nombre de archivo es requerido.");
        }
    }
}

internal sealed class UploadGirlGaleryImageCommandHandler(
    IGaleryRepository repository,
    IImgBBService imgBBService)
    : IRequestHandler<UploadGirlGaleryImageCommand, Result>
{
    public async Task<Result> Handle(UploadGirlGaleryImageCommand request, CancellationToken cancellationToken)
    {
        var galery = await repository.GetGirlGaleryById(request.GaleryId);
        if (galery is null)
            return Errors.NotFound("Galeria no encontrada.");

        using var stream = new MemoryStream(request.ImageData);
        var fileNameWithWebpExtension = Path.GetFileNameWithoutExtension(request.FileName) + ".webp";
        var uploadResult = await imgBBService.UploadImageAsync(stream, fileNameWithWebpExtension);

        if (uploadResult is null)
            return Errors.BadRequest("Error al subir la imagen a ImgBB.");

        await repository.UpdateGirlGaleryImage(request.GaleryId, uploadResult.Url);

        return Results.NoContent();
    }
}
