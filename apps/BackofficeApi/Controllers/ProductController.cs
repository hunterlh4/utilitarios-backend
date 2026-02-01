using BackofficeApi.Shared.Extensions;
using BackofficeApi.Shared.Filters;
using BackofficeCore.Application.Features.Products.Actions;
using BackofficeCore.Application.Features.Products.Dtos;
using BackofficeCore.Application.Features.Products.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackofficeApi.Controllers;

[Route("api/products")]
[ApiController]
[AuthorizeJwt]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] GetProductsQuery query)
    {
        var response = await sender.Send(query);

        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductDto>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var response = await sender.Send(command);

        return response.ToActionResult();
    }

    [HttpGet("{productId:int}")]
    public async Task<ActionResult<ProductDto>> GetProductById([FromRoute] int productId)
    {
        var response = await sender.Send(new GetProductByIdQuery(productId));

        return response.ToActionResult();
    }

    [HttpPut("{productId:int}")]
    public async Task<ActionResult> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductRequest payload)
    {
        var response = await sender.Send(new UpdateProductCommand(productId)
        {
            Sku = payload.Sku,
            Name = payload.Name,
            MinimumStock = payload.MinimumStock
        });

        return response.ToActionResult();
    }

    [HttpGet("{productId:int}/generate-qr")]
    public async Task<ActionResult<QrProductDto>> GenerateQrProductById([FromRoute] int productId)
    {
        var response = await sender.Send(new GenerateQrProductByIdQuery(productId));

        return response.ToActionResult();
    }
}
