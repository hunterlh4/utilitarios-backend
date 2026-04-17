using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Hentais.Actions;

public record UploadHentaiImageCommand : IRequest<Result>
{
    public int HentaiId { get; init; }
    public byte[] ImageData { get; init; } = [];
    public string FileName { get; init; } = string.Empty;

    public sealed class Validator : AbstractValidator<UploadHentaiImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.HentaiId).GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.ImageData).NotEmpty().WithMessage("La imagen es requerida.");
            RuleFor(x => x.FileName).NotEmpty().WithMessage("El nombre de archivo es requerido.");
        }
    }
}

internal sealed class UploadHentaiImageCommandHandler(
    IHentaiRepository hentaiRepository,
    IImgBBService imgBBService)
    : IRequestHandler<UploadHentaiImageCommand, Result>
{
    public async Task<Result> Handle(UploadHentaiImageCommand request, CancellationToken cancellationToken)
    {
        var hentai = await hentaiRepository.GetHentaiById(request.HentaiId);
        if (hentai is null)
            return Errors.NotFound("Hentai no encontrado.");

        using var stream = new MemoryStream(request.ImageData);
        var fileNameWithWebpExtension = Path.GetFileNameWithoutExtension(request.FileName) + ".webp";
        var uploadResult = await imgBBService.UploadImageAsync(stream, fileNameWithWebpExtension);

        if (uploadResult is null)
            return Errors.BadRequest("Error al subir la imagen a ImgBB.");

        hentai.Image = uploadResult.Url;
        await hentaiRepository.UpdateHentai(hentai);

        return Results.NoContent();
    }
}