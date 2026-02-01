namespace BackofficeCore.Application.Features.Products.Dtos;

public class AvailableProductForAssignmentDto
{
    public int Id { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public ProductBrandDto? Brand { get; set; }
    public bool IsAssigned { get; set; } // Indica si ya está asignado como hijo de este padre
}
