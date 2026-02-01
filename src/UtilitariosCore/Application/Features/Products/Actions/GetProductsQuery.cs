using UtilitariosCore.Application.Features.Products.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using MediatR;

namespace UtilitariosCore.Application.Features.Products.Actions;

public class GetProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
{
    internal sealed class Handler(IProductRepository productRepository) : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
    {
        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var items = await productRepository.GetAllProducts();

            return items.Select(x => new ProductDto
            {
                Id = x.Id,
                Sku = x.Sku,
                Name = x.Name,
                ProductGroupId = x.ProductGroupId,
                PresentationUnit = x.PresentationUnit,
                PresentationSize = x.PresentationSize,
                Unit = x.Unit,
                Size = x.Size,
                ProductType = x.ProductType,
                Description = x.Description,
                MinimumStock = x.MinimumStock,
                GroupStatus = x.GroupStatus
            }).ToList();
        }
    }
}