using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Common.Exceptions;

namespace OrderFlow.Application.Products.DeactivateProduct;

public sealed class DeactivateProductService
{
    private readonly IProductRepository _productRepository;

    public DeactivateProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetTrackedByIdAsync(id, cancellationToken);
        if (product is null)
            throw new NotFoundException($"Product '{id}' was not found.");

        product.Deactivate();
        await _productRepository.SaveChangesAsync(cancellationToken);
    }
}
