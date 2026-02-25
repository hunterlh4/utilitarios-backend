using System.Text.Json;
using Microsoft.Extensions.Configuration;
using UtilitariosCore.Application.Features.Media.Dtos;
using UtilitariosCore.Domain.Interfaces;

namespace UtilitariosCore.Infrastructure.Services.ImageUpload;

public class ImgBBService : IImgBBService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ImgBBService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Providers:ImgBB:ApiKey"] ?? throw new Exception("ImgBB API Key not configured");
    }

    public async Task<ImgBBUploadResponse?> UploadImageAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new ArgumentException("File is required");
        }

        // Validar tipo de archivo
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"Invalid file type. Allowed: {string.Join(", ", allowedExtensions)}");
        }

        // Convertir a base64
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        var base64 = Convert.ToBase64String(memoryStream.ToArray());

        // Preparar request
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(base64), "image");

        var response = await _httpClient.PostAsync(
            $"https://api.imgbb.com/1/upload?key={_apiKey}",
            formData
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"ImgBB upload failed: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ImgBBApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result?.Success != true || result.Data == null)
        {
            throw new Exception("ImgBB upload failed");
        }

        return new ImgBBUploadResponse
        {
            Url = result.Data.Url,
            DisplayUrl = result.Data.DisplayUrl,
            Thumbnail = result.Data.Thumb?.Url ?? result.Data.Medium?.Url ?? result.Data.Url,
            DeleteUrl = result.Data.DeleteUrl ?? string.Empty
        };
    }

    public async Task DeleteImageAsync(string deleteUrl)
    {
        if (!string.IsNullOrWhiteSpace(deleteUrl))
            await _httpClient.GetAsync(deleteUrl);
    }
}