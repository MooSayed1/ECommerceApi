using AutoMapper;
using Domain.Entities.OrderEntities;
using Shared.Dtos.OrderDtos;

namespace Services.MappingProfiles;

public class OrderProfile :  Profile
{
    public OrderProfile()
    {
        CreateMap<Order,OrderResultDto>()
            .ForMember(o=>o.DeliveryMethod,opt=>opt.MapFrom(d=>d.DeliveryMethod.ShortName))
            .ReverseMap();
        
    }
}