namespace Shared.Dtos.OrderDtos;

public class DeliveryMethodDto
{
    public int Id { get; init; }
    public string ShortName { get; init; }
    public string Description { get; init; }
    public string DeliveryDate { get; init; }
    public decimal Price { get; init; }
}