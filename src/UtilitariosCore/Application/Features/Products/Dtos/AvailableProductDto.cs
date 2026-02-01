namespace UtilitariosCore.Application.Features.Products.Dtos;

public class AvailableProductDto
{
    public int ProductId { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal TotalStock { get; set; }
    public decimal AssignedStock { get; set; }
    public decimal AvailableStock { get; set; }
    public string? Unit { get; set; }
    public decimal AveragePrice { get; set; }
    public int ProductType { get; set; }
    
    // Campos adicionales del producto
    public int? ProductGroupId { get; set; }
    public int? PresentationUnit { get; set; }
    public decimal? PresentationSize { get; set; }
    public int? ProductUnit { get; set; }
    public decimal? ProductSize { get; set; }
}
