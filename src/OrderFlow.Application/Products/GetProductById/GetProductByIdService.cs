using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Common.Exceptions;

namespace OrderFlow.Application.Products.GetProductById;

public sealed class GetProductByIdService
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Product '{id}' was not found.");
    }
}
