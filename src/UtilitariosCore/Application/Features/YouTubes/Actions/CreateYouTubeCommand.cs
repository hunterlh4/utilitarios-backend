using System.Text.Json;
using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.YouTubes.Actions;

public record CreateYouTubeCommand : IRequest<Result<int>>
{
    public string Url { get; init; } = string.Empty;
    public YouTubeCategory Category { get; init; }
}

internal sealed class CreateYouTubeCommandValidator : AbstractValidator<CreateYouTubeCommand>
{
    public CreateYouTubeCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("La URL es requerida.")
            .Must(url => url.Contains("youtube.com") || url.Contains("youtu.be"))
            .WithMessage("Debe ser una URL válida de YouTube.");
        RuleFor(x => x.Category).IsInEnum().WithMessage("Categoría inválida.");
    }
}

internal sealed class CreateYouTubeCommandHandler(
    IYouTubeRepository youTubeRepository,
    IHttpClientFactory httpClientFactory)
    : IRequestHandler<CreateYouTubeCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateYouTubeCommand request, CancellationToken cancellationToken)
    {
        if (await youTubeRepository.ExistsByUrl(request.Url))
            return Errors.BadRequest("Esta URL de YouTube ya existe.");

        var client = httpClientFactory.CreateClient();
        var oEmbedUrl = $"https://www.youtube.com/oembed?url={Uri.EscapeDataString(request.Url)}&format=json";

        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(oEmbedUrl, cancellationToken);
        }
        catch
        {
            return Errors.InternalServerError("No se pudo conectar con la API de YouTube.");
        }

        if (!response.IsSuccessStatusCode)
            return Errors.BadRequest("URL de YouTube no válida o no accesible.");

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var oembed = JsonSerializer.Deserialize<YouTubeOEmbedResponse>(json);

        if (oembed is null)
            return Errors.InternalServerError("Error al procesar la respuesta de YouTube.");

        var item = new YouTube
        {
            Url = request.Url,
            Title = oembed.Title ?? string.Empty,
            AuthorName = oembed.AuthorName,
            AuthorUrl = oembed.AuthorUrl,
            Type = oembed.Type,
            Height = oembed.Height,
            Width = oembed.Width,
            Version = oembed.Version,
            ProviderName = oembed.ProviderName,
            ProviderUrl = oembed.ProviderUrl,
            ThumbnailHeight = oembed.ThumbnailHeight,
            ThumbnailWidth = oembed.ThumbnailWidth,
            ThumbnailUrl = oembed.ThumbnailUrl,
            Html = oembed.Html,
            Category = request.Category,
            CreatedAt = DateTime.Now
        };

        return await youTubeRepository.Create(item);
    }
}

