using Shared.Dtos.CustomerBasketDtos;

namespace Services.Abstraction.Interfaces;

public interface IPaymentService
{
    // Create or update payment intent
    public Task<BasketDto>CreatOrUpdateBasketIntentAsync(string basketId);
}