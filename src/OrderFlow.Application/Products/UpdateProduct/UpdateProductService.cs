using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Common.Exceptions;

namespace OrderFlow.Application.Products.UpdateProduct;

public sealed class UpdateProductService
{
    private readonly IProductRepository _productRepository;

    public UpdateProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> ExecuteAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetTrackedByIdAsync(id, cancellationToken);
        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        // Invalid SKUs are rejected by the domain below without a database lookup.
        if (!string.IsNullOrWhiteSpace(request.Sku) &&
            await _productRepository.ExistsBySkuExceptAsync(request.Sku, id, cancellationToken))
            throw new ConflictException($"Product with SKU '{request.Sku}' already exists.");

        product.ChangeName(request.Name);
        product.ChangeSku(request.Sku);
        product.ChangePrice(request.Price);
        product.ChangeStock(request.Stock);

        if (request.IsActive)
            product.Activate();
        else
            product.Deactivate();

        await _productRepository.SaveChangesAsync(cancellationToken);

        return new ProductResponse(
            product.Id, product.Name, product.Sku, product.Price,
            product.Stock, product.IsActive, product.CreatedAt);
    }
}
