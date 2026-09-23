using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal TotalPrice => UnitPrice * Quantity;

    private OrderItem()
    {
        ProductName = string.Empty;
    }

    internal OrderItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException(
                "Product ID is required.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException(
                "Product name is required.");

        if (unitPrice <= 0)
            throw new DomainException(
                "Unit price must be greater than zero.");

        if (quantity <= 0)
            throw new DomainException(
                "Quantity must be greater than zero.");

        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}