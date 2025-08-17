using AutoMapper;
using Domain.Entities;
using Shared.Dtos.CustomerBasketDtos;

namespace Services.MappingProfiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<BasketDto,CustomerBasket>().ReverseMap();
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
}