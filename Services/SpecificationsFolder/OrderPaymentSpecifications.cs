using Domain.Contracts;
using Domain.Entities.OrderEntities;

namespace Services.SpecificationsFolder;

public class OrderPaymentSpecifications : Specifications<Order>
{
    public OrderPaymentSpecifications(string paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
    {
        
    }
}