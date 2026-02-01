using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;
using MediatR;

namespace UtilitariosCore.Application.Features.Products.Actions;

public record UpdateProductCommand(int Id) : IRequest<Result>
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal? MinimumStock { get; set; }

    internal sealed class Handler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, Result>
    {
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var item = await productRepository.GetProductById(request.Id);

            if (item is null)
            {
                return Errors.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                return Errors.BadRequest();
            }

            if (item.Sku != request.Sku)
            {
                var product = await productRepository.GetProductBySku(request.Sku);

                if (product != null)
                {
                    return Errors.BadRequest($"Product Sku {item.Sku} already exists.");
                }

                item.Sku = request.Sku;
            }

            item.Name = request.Name;
            item.MinimumStock = request.MinimumStock;
            item.UpdatedAt = DateTimeOffset.UtcNow;

            var updated = await productRepository.UpdateProduct(item);

            if (!updated)
            {
                return Errors.BadRequest();
            }

            await productRepository.UpdateManyItems(item);

            return Results.NoContent();
        }
    }
}