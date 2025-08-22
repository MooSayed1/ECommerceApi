using Domain.Entities.OrderEntities;
using Shared.Enums;

namespace Shared.Dtos.OrderDtos;
using ShippingAddress = Domain.Entities.OrderEntities.Address;

public record OrderResultDto
{
    public Guid Id { get; init; }
    public string UserEmail { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
    public string PaymentStatus { get; init; } 
    public string DeliveryMethod { get; init; } // configure the auto mapper for this one
    public decimal SubTotal { get; init; }
    DateTimeOffset OrderDate { get; init; } =  DateTimeOffset.Now;
    public string PaymentIntendId { get; init; } = string.Empty;
    public int Total { get; init; }
}