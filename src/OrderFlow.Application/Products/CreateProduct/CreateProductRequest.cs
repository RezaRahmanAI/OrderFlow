namespace OrderFlow.Application.Products.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    int Stock);