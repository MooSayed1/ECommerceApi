using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;

namespace Services;

public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
{
    public async Task<BasketDto?> GetBasketAsync(string id)
    {
        var basket = await basketRepository.GetBasketAsync(id) ?? throw new BasketNotFoundException(id);

        return mapper.Map<BasketDto>(basket);
    }

    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await basketRepository.DeleteBasketItemAsync(id);
    }

    public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
    {
        var customerBasket = await basketRepository.UpdateBasketAsync(mapper.Map<CustomerBasket>(basketDto)) ??
                             throw new BasketNotFoundException(basketDto.Id);
        var resultCustomerDto = mapper.Map<BasketDto>(customerBasket);
        return resultCustomerDto;
    }
}