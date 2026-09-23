using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Products;
using OrderFlow.Application.Products.UpdateProduct;
using OrderFlow.Application.Products.CreateProduct;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductService _createProductService;
    private readonly UpdateProductService _updateProductService;

    public ProductsController(
        CreateProductService createProductService,
        UpdateProductService updateProductService)
    {
        _createProductService = createProductService;
        _updateProductService = updateProductService;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await _updateProductService.ExecuteAsync(id, request, cancellationToken);
            if (product is null)
                return NotFound(new { message = $"Product '{id}' was not found." });

            return Ok(product);
        }
        catch (DomainException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _createProductService.ExecuteAsync(
                request,
                cancellationToken);

            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (DomainException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}
