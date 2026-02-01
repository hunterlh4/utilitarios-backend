namespace BackofficeCore.Application.Features.Products.Models;

public class UpdateProductRequest
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal? MinimumStock { get; set; }
}