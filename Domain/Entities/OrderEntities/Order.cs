using Shared.Enums;

namespace Domain.Entities.OrderEntities;

using ShippingAddress = Domain.Entities.OrderEntities.Address;

public class Order : BaseEntity<Guid>
{
    public Order(string userEmail,
        ShippingAddress shippingAddress,
        DeliveryMethod deliveryMethod,
        decimal subTotal,
        ICollection<OrderItem> orderItems
        )
    {
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
        OrderItems = orderItems;
    }

    public Order()
    {
        
    }

    public string UserEmail { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
    public DeliveryMethod DeliveryMethod { get; set; }
    public int? DeliveryMethodId { get; set; }
    public decimal SubTotal { get; set; }
    DateTimeOffset OrderDate { get; set; } =  DateTimeOffset.Now;
    public string PaymentIntendId { get; set; } = string.Empty;
}