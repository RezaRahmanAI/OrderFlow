namespace OrderFlow.Application.Products.CreateProduct;

public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    bool IsActive,
    DateTime CreatedAt);