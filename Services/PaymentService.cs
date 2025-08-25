using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;
using Stripe;
using Product = Domain.Entities.Product;

namespace Services;

public class PaymentService(
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration) : IPaymentService
{
    public async Task<BasketDto> CreatOrUpdateBasketIntentAsync(string basketId)
    {
        // Api Key Configuration
        StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
        // GetBasket
        var basket = await basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);

        // validate the price of items in the basket
        foreach (var item in basket.Items)
        {
            var product = await unitOfWork.GetRepo<Product, int>().GetByIdAsync(item.Id);
            if (product == null) throw new ProductNotFoundException(item.Id);
            basket.ShippingPrice = product.Price;
        }

        if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodForBasketNotFoundException(basketId);

        var deliveryMethod = await unitOfWork.GetRepo<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value)
                             ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

        basket.ShippingPrice = deliveryMethod.Price;

        var amount = (long)(basket.Items.Sum(i => i.Price * i.Quantity) + deliveryMethod.Price) * 100;
        var service = new PaymentIntentService();
        // If he wants to create or update
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // Create
            var createOptions = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "USD",
                PaymentMethodTypes = {"card"}
            };
           var paymentIntent = await service.CreateAsync(createOptions);
           basket.PaymentIntentId = paymentIntent.Id;
           basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            // Update
            var updateOptions = new PaymentIntentUpdateOptions
            {
                Amount = amount,
            };
            
            var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
        }

        await basketRepository.UpdateBasketAsync(basket); // Price validations
        return mapper.Map<BasketDto>(basket);
    }
}