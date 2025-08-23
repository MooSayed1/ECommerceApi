using AutoMapper;
using Domain.Entities.OrderEntities;
using Shared.Dtos.OrderDtos;

namespace Services.MappingProfiles;

public class OrderProfile :  Profile
{
    public OrderProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
        
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(o => o.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
            .ForMember(o => o.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(o => o.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl)).ReverseMap();

        CreateMap<Order, OrderResultDto>()
            .ForMember(o => o.DeliveryMethod, opt => opt.MapFrom(d => d.DeliveryMethod.ShortName))
            .ForMember(o => o.PaymentStatus, opt => opt.MapFrom(d => d.PaymentStatus.ToString()))
            .ForMember(o=>o.Total,opt=>opt.MapFrom(src=>src.SubTotal+src.DeliveryMethod.Price));

    }
}