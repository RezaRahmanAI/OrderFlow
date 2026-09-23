using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions;

public interface IProductRepository
{
    Task<Product?> GetTrackedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySkuExceptAsync(
        string sku,
        Guid productId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default);
}
