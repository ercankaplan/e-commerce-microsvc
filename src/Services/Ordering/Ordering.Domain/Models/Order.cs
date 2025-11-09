


namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private List<OrderItem> _orderItems = new();

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = default!;

    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;

    public Address BillingAddress { get; private set; } = default!;

    public Payment Payment { get; private set; } = default!;

    public OrderStatus OrderStats { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }


    public static Order Create(OrderId orderId, CustomerId customerId, OrderName orderName,
        Address shippingAddress, Address billingAddress, Payment payment)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderName.Value);

        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order);

        return order;
    }

    public void Update(OrderName orderName, Address shippingAddress,
        Address billingAddress, Payment payment)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderName.Value);
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {


        var orderItem = OrderItem.Create(Id, productId, quantity, price);
        _orderItems.Add(orderItem);

        //AddDomainEvent(new OrderItemAddedEvent(this, orderItem));
    }

    public void Remove(OrderItemId orderItemId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (orderItem != null)
        {
            _orderItems.Remove(orderItem);
            //AddDomainEvent(new OrderItemRemovedEvent(this, orderItem));
        }
    }

}



