using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrderFlow.Domain.Enums;
using OrderFlow.Application.Products.DeactivateProduct;
using OrderFlow.Application.Products.GetProducts;
using OrderFlow.Application.Products.GetProductById;
using OrderFlow.Application.Products;
using OrderFlow.Application.Products.UpdateProduct;
using OrderFlow.Application.Products.CreateProduct;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly CreateProductService _createProductService;
    private readonly UpdateProductService _updateProductService;
    private readonly DeactivateProductService _deactivateProductService;
    private readonly GetProductsService _getProductsService;
    private readonly GetProductByIdService _getProductByIdService;

    public ProductsController(
        CreateProductService createProductService,
        UpdateProductService updateProductService,
        DeactivateProductService deactivateProductService,
        GetProductsService getProductsService,
        GetProductByIdService getProductByIdService)
    {
        _createProductService = createProductService;
        _updateProductService = updateProductService;
        _deactivateProductService = deactivateProductService;
        _getProductsService = getProductsService;
        _getProductByIdService = getProductByIdService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _getProductsService.ExecuteAsync(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _getProductByIdService.ExecuteAsync(id, cancellationToken);

        return Ok(product);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _deactivateProductService.ExecuteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _updateProductService.ExecuteAsync(id, request, cancellationToken);
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _createProductService.ExecuteAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
