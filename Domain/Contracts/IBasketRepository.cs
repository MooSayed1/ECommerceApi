using Domain.Entities;

namespace Domain.Contracts;

public interface IBasketRepository
{
    public Task<CustomerBasket?> GetBasketAsync(string customerId);
    
    public Task<bool> DeleteBasketItemAsync(string customerId);
    
    public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeout = null);
}