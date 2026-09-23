namespace OrderFlow.Application.Products.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    bool IsActive);
