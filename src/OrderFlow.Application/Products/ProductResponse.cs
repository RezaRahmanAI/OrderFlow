namespace OrderFlow.Application.Products;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    bool IsActive,
    DateTime CreatedAt);
