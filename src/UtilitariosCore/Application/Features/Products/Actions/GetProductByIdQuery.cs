using UtilitariosCore.Application.Features.Products.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using MediatR;

namespace UtilitariosCore.Application.Features.Products.Actions;

public record GetProductByIdQuery(int Id) : IRequest<Result<ProductDto>>
{
    internal sealed class Handler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await productRepository.GetProductById(request.Id);

            if (item == null)
            {
                return Errors.NotFound();
            }

            var response = new ProductDto
            {
                Id = item.Id,
                Sku = item.Sku,
                Name = item.Name,
                ProductGroupId = item.ProductGroupId,
                PresentationUnit = item.PresentationUnit,
                PresentationSize = item.PresentationSize,
                Unit = item.Unit,
                Size = item.Size,
                ProductType = item.ProductType,
                Description = item.Description,
                MinimumStock = item.MinimumStock
            };

            return response;
        }
    }
}