using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Sku { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Sku = string.Empty;
    }

    public Product(
        string name,
        string sku,
        decimal price,
        int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Product name is required.");

        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException(
                "Product SKU is required.");

        if (price <= 0)
            throw new DomainException(
                "Product price must be greater than zero.");

        if (stock < 0)
            throw new DomainException(
                "Stock cannot be negative.");

        Id = Guid.NewGuid();

        Name = name.Trim();

        Sku = sku.Trim().ToUpperInvariant();

        Price = price;

        Stock = stock;

        IsActive = true;

        CreatedAt = DateTime.UtcNow;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(
                "Quantity must be greater than zero.");

        if (quantity > Stock)
            throw new DomainException(
                "Insufficient stock available.");

        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(
                "Quantity must be greater than zero.");

        Stock += quantity;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException(
                "Product price must be greater than zero.");

        Price = newPrice;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        Name = name.Trim();
    }

    public void ChangeSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU is required.");

        Sku = sku.Trim().ToUpperInvariant();
    }

    public void ChangeStock(int stock)
    {
        if (stock < 0)
            throw new DomainException("Stock cannot be negative.");

        Stock = stock;
    }

    public void Activate()
    {
        IsActive = true;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}
