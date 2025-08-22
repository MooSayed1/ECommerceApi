namespace Domain.Entities.OrderEntities;

public class ProductInOrderItem
{
    public ProductInOrderItem()
    {
        
    }
    public ProductInOrderItem(string productId, string productName, string pictureUrl)
    {
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
    }

    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string PictureUrl { get; set; }
}