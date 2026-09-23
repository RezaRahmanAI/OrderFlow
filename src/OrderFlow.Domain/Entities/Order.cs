using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items
        => _items.AsReadOnly();

    public decimal TotalAmount
        => _items.Sum(x => x.TotalPrice);

    private Order()
    {
    }

    public Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException(
                "Customer ID is required.");

        Id = Guid.NewGuid();

        CustomerId = customerId;

        Status = OrderStatus.Pending;

        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(
        Product product,
        int quantity)
    {
        if (product is null)
            throw new DomainException(
                "Product is required.");

        if (!product.IsActive)
            throw new DomainException(
                "Inactive product cannot be ordered.");

        if (quantity <= 0)
            throw new DomainException(
                "Quantity must be greater than zero.");

        if (product.Stock < quantity)
            throw new DomainException(
                "Insufficient product stock.");

        var existingItem =
            _items.FirstOrDefault(
                x => x.ProductId == product.Id);

        if (existingItem is not null)
            throw new DomainException(
                "Product already exists in the order.");

        var item = new OrderItem(
            product.Id,
            product.Name,
            product.Price,
            quantity);

        _items.Add(item);
    }

    public void RemoveItem(Guid productId)
    {
        var item =
            _items.FirstOrDefault(
                x => x.ProductId == productId);

        if (item is null)
            throw new DomainException(
                "Product does not exist in the order.");

        _items.Remove(item);
    }

    public void Confirm()
    {
        if (_items.Count == 0)
            throw new DomainException(
                "Cannot confirm an empty order.");

        if (Status != OrderStatus.Pending)
            throw new DomainException(
                "Only pending orders can be confirmed.");

        Status = OrderStatus.Confirmed;
    }

    public void MarkAsProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException(
                "Only confirmed orders can be processed.");

        Status = OrderStatus.Processing;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Processing)
            throw new DomainException(
                "Only processing orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException(
                "Only shipped orders can be delivered.");

        Status = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException(
                "Delivered order cannot be cancelled.");

        if (Status == OrderStatus.Cancelled)
            throw new DomainException(
                "Order is already cancelled.");

        Status = OrderStatus.Cancelled;
    }
}