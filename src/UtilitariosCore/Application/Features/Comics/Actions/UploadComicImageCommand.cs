using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Comics.Actions;

public record UploadComicImageCommand : IRequest<Result>
{
    public int ComicId { get; init; }
    public byte[] ImageData { get; init; } = [];
    public string FileName { get; init; } = string.Empty;

    public sealed class Validator : AbstractValidator<UploadComicImageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ComicId).GreaterThan(0);
            RuleFor(x => x.ImageData).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}

internal sealed class UploadComicImageCommandHandler(
    IComicRepository repository,
    IImgBBService imgBBService)
    : IRequestHandler<UploadComicImageCommand, Result>
{
    public async Task<Result> Handle(UploadComicImageCommand request, CancellationToken cancellationToken)
    {
        var comic = await repository.GetComicById(request.ComicId);
        if (comic is null) return Errors.NotFound("Comic no encontrado.");

        using var stream = new MemoryStream(request.ImageData);
        var fileName = Path.GetFileNameWithoutExtension(request.FileName) + ".webp";
        var uploadResult = await imgBBService.UploadImageAsync(stream, fileName);

        if (uploadResult is null) return Errors.BadRequest("Error al subir la imagen.");

        comic.Image = uploadResult.Url;
        await repository.UpdateComic(comic);
        return Results.NoContent();
    }
}
