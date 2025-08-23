namespace Shared.Dtos.OrderDtos;

public record OrderRequest()
{
    public string BasketId { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public int DeliveryMethodId { get; init; }
}