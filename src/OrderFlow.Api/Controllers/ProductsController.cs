using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Products.CreateProduct;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductService _createProductService;

    public ProductsController(CreateProductService createProductService)
    {
        _createProductService = createProductService;
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
