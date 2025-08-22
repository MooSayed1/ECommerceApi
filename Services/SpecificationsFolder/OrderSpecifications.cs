using Domain.Contracts;
using Domain.Entities.OrderEntities;

namespace Services.SpecificationsFolder;

public class OrderSpecifications : Specifications<Order> 
{
    public OrderSpecifications(Guid id) : base(order => order.Id == id)
    {
        IncludeExpressions.Add(x => x.OrderItems);
        IncludeExpressions.Add(x => x.DeliveryMethod);
        
    }

    public OrderSpecifications(string email) : base(order => order.UserEmail == email)
    {
        IncludeExpressions.Add(x => x.OrderItems);
        IncludeExpressions.Add(x => x.DeliveryMethod);
    }
}