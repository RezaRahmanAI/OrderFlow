using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions;
using OrderFlow.Application.Products;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        var normalizedSku = sku.Trim().ToUpperInvariant();

        return await _dbContext.Products.AnyAsync(
            x => x.Sku == normalizedSku,
            cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetTrackedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AsTracking().SingleOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public async Task<bool> ExistsBySkuExceptAsync(
        string sku,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var normalizedSku = sku.Trim().ToUpperInvariant();

        return await _dbContext.Products.AnyAsync(
            x => x.Sku == normalizedSku && x.Id != productId,
            cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => new ProductResponse(
                x.Id, x.Name, x.Sku, x.Price, x.Stock, x.IsActive, x.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AsNoTracking()
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new ProductResponse(
                x.Id, x.Name, x.Sku, x.Price, x.Stock, x.IsActive, x.CreatedAt))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
