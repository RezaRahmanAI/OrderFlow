using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions;

public interface IProductRepository
{
    Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default);
}