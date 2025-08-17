namespace Domain.Entities;

public class CustomerBasket
{
    public string Id { get; set; }
    public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
}