namespace Shared.Dtos.OrderDtos;

public class DeliveryMethodDto
{
    public int Id { get; init; }
    public string ShortName { get; init; }
    public string Description { get; init; }
    public string DeliveryTime { get; init; }
    public decimal Cost { get; init; }
}