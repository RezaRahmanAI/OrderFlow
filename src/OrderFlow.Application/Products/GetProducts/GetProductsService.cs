using OrderFlow.Application.Abstractions;

namespace OrderFlow.Application.Products.GetProducts;

public sealed class GetProductsService
{
    private readonly IProductRepository _productRepository;

    public GetProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IReadOnlyList<ProductResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _productRepository.GetAllAsync(cancellationToken);
    }
}
