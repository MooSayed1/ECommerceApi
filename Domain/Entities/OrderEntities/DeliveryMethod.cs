namespace Domain.Entities.OrderEntities;

public class DeliveryMethod : BaseEntity<int>
{
    public DeliveryMethod()
    {
        
    }
    public DeliveryMethod(string shortName, string description, string deliveryDate, decimal price)
    {
        ShortName = shortName;
        Description = description;
        DeliveryDate = deliveryDate;
        Price = price;
    }

    public string ShortName { get; set; }
    public string Description { get; set; }
    public string DeliveryDate { get; set; }
    public decimal Price { get; set; }
}