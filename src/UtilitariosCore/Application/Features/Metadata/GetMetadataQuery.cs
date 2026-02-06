using MediatR;
using UtilitariosCore.Shared.Responses;
using System.Text.Json;

namespace UtilitariosCore.Application.Features.Metadata;

public record GetMetadataQuery(string Url) : IRequest<Result<MetadataResponse>>
{
    internal sealed class Handler : IRequestHandler<GetMetadataQuery, Result<MetadataResponse>>
    {
        private readonly HttpClient _httpClient;

        public Handler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Result<MetadataResponse>> Handle(GetMetadataQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return Errors.BadRequest("URL is required.");
            }

            try
            {
                var encodedUrl = Uri.EscapeDataString(request.Url);
                var apiUrl = $"https://api.microlink.io/?url={encodedUrl}&palette=true&audio=true&video=true&iframe=true";

                var response = await _httpClient.GetAsync(apiUrl, cancellationToken);

             

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var microlinkResponse = JsonSerializer.Deserialize<MicrolinkApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

               
                var data = microlinkResponse.Data;

                var metadataResponse = new MetadataResponse
                {
                    Status = "success",
                    Data = new MetadataData
                    {
                        Title = data.Title ?? string.Empty,
                        Description = data.Description,
                        Image = data.Image != null ? new ImageData { Url = data.Image.Url ?? string.Empty } : null,
                        Url = request.Url
                    }
                };

                return metadataResponse;
            }
            catch (Exception ex)
            {
                return Errors.BadRequest($"Error fetching metadata: {ex.Message}");
            }
        }
    }
}

public class MetadataResponse
{
    public string Status { get; set; } = string.Empty;
    public MetadataData Data { get; set; } = new();
}

public class MetadataData
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ImageData? Image { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class ImageData
{
    public string Url { get; set; } = string.Empty;
}

// Clases para deserializar la respuesta de Microlink
internal class MicrolinkApiResponse
{
    public string? Status { get; set; }
    public MicrolinkData? Data { get; set; }
}

internal class MicrolinkData
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public MicrolinkImage? Image { get; set; }
}

internal class MicrolinkImage
{
    public string? Url { get; set; }
}
