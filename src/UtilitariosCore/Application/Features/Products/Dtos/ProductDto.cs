using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Products.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public int? ProductGroupId { get; set; }
    public MeasurementUnit? PresentationUnit { get; set; }
    public decimal? PresentationSize { get; set; }
    public MeasurementUnit? Unit { get; set; }
    public decimal? Size { get; set; }
    public ProductType ProductType { get; set; }
    public string? Description { get; set; }
    public decimal? MinimumStock { get; set; }
    public ProductGroupStatus GroupStatus { get; set; }
    public ProductBrandDto? Brand { get; set; }
    public List<ProductCategoryDto> Categories { get; set; } = [];
}

public class ProductBrandDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ProductCategoryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
