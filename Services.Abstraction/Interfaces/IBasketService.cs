using Shared.Dtos.CustomerBasketDtos;

namespace Services.Abstraction.Interfaces;

public interface IBasketService
{
    public Task<BasketDto?> GetBasketAsync(string id);
    public Task<bool> DeleteBasketAsync(string id);
    public Task<BasketDto?> UpdateBasketAsync(BasketDto basketDto);
}