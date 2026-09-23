using OrderFlow.Application.Abstractions;
using OrderFlow.Domain.Entities;
using OrderFlow.Application.Common.Exceptions;

namespace OrderFlow.Application.Products.CreateProduct;

public sealed class CreateProductService
{
    private readonly IProductRepository _productRepository;

    public CreateProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductResponse> ExecuteAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var skuExists =
            await _productRepository.ExistsBySkuAsync(
                request.Sku,
                cancellationToken);

        if (skuExists)
        {
            throw new ConflictException(
                $"Product with SKU '{request.Sku}' already exists.");
        }

        var product = new Product(
            request.Name,
            request.Sku,
            request.Price,
            request.Stock);

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        return new CreateProductResponse(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.Stock,
            product.IsActive,
            product.CreatedAt);
    }
}
