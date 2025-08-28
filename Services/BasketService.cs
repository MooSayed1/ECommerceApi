using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;

namespace Services;

public class BasketService(
    IBasketRepository basketRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IConfiguration configuration) : IBasketService
{
    public async Task<BasketDto?> GetBasketAsync(string id)
    {
        var basket = await basketRepository.GetBasketAsync(id) ?? throw new BasketNotFoundException(id);

        foreach (var item in basket.Items)
        {
            var product = await unitOfWork.GetRepo<Product, int>().GetByIdAsync(item.Id);
            item.PictureUrl = $"{configuration["JwtOptions:Issuer"]}/{product.PictureUrl}";
        }

        return mapper.Map<BasketDto>(basket);
    }

    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await basketRepository.DeleteBasketItemAsync(id);
    }

    public async Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto)
    {
        var customerBasket = await basketRepository.UpdateBasketAsync(mapper.Map<CustomerBasket>(basketDto)) ??
                             throw new BasketNotFoundException("NoIdea");
        var resultCustomerDto = mapper.Map<BasketDto>(customerBasket);
        return resultCustomerDto;
    }
}