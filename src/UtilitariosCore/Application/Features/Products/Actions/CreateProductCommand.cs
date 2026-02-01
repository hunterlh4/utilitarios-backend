using UtilitariosCore.Application.Features.Products.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;
using MediatR;

namespace UtilitariosCore.Application.Features.Products.Actions;

public class CreateProductCommand : IRequest<Result<CreateProductDto>>
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public int? ProductGroupId { get; set; }
    public MeasurementUnit? PresentationUnit { get; set; }
    public decimal? PresentationSize { get; set; }
    public ProductType ProductType { get; set; }
    public string? Description { get; set; }
    public decimal? MinimumStock { get; set; }

    internal sealed class Handler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Result<CreateProductDto>>
    {
        public async Task<Result<CreateProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                return Errors.BadRequest("Product Sku is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Errors.BadRequest("Product name is required.");
            }

            var item = await productRepository.GetProductBySku(request.Sku);

            if (item != null)
            {
                return Errors.BadRequest($"Product Sku {item.Sku} already exists.");
            }

            // Calcular Unit y Size basado en PresentationUnit y PresentationSize
            MeasurementUnit? unit = null;
            decimal? size = null;

            if (request.PresentationUnit.HasValue && request.PresentationSize.HasValue)
            {
                // Unit es la unidad base de almacenamiento
                unit = UnitConversionHelper.GetStorageUnit(request.PresentationUnit.Value);
                
                // Size es la conversión de PresentationSize a la unidad base
                // Ejemplo: PresentationUnit=Liter, PresentationSize=5 ? Unit=Milliliter, Size=5000
                size = UnitConversionHelper.ConvertToStorageUnit(request.PresentationSize.Value, request.PresentationUnit.Value);
            }

            var newItem = new Product
            {
                Sku = request.Sku,
                Name = request.Name,
                ProductGroupId = request.ProductGroupId,
                PresentationUnit = request.PresentationUnit,
                PresentationSize = request.PresentationSize,
                Unit = unit,
                Size = size,
                ProductType = request.ProductType,
                Description = request.Description,
                MinimumStock = request.MinimumStock,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var itemId = await productRepository.CreateProduct(newItem);

            return Results.Created(new CreateProductDto
            {
                Id = itemId
            });
        }
    }
}
