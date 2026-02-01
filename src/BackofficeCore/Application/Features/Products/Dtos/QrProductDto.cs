namespace BackofficeCore.Application.Features.Products.Dtos;

public record QrProductDto
{
    public string? FileName { get; set; }
    public string? Base64 { get; set; }
}