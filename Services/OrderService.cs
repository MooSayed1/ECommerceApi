using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Services.Abstraction.Interfaces;
using Services.SpecificationsFolder;
using Shared.Dtos.OrderDtos;
using Address = Domain.Entities.OrderEntities.Address;

namespace Services;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IOrderService
{
    public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
    {
        var order = await unitOfWork.GetRepo<Order, Guid>().GetByIdAsync(new OrderSpecifications(id)) ??
                    throw new OrderNotFoundException(id);
        var mappedOrder = mapper.Map<OrderResultDto>(order); // I want to make profile
        return mappedOrder;
    }

    public async Task<IEnumerable<OrderResultDto>> GetAllOrdersByEmailAsync(string? email)
    {
        var orders = await unitOfWork.GetRepo<Order, Guid>().GetAllAsync(new OrderSpecifications(email!));
        var mappedOrders = mapper.Map<IEnumerable<OrderResultDto>>(orders);
        return mappedOrders;
    }

    public async Task<OrderResultDto> AddOrderAsync(OrderRequest request, string email)
    {
        // Address , shipping address map
        var address = mapper.Map<Address>(request.ShippingAddress);
        // OrderItems ==> Basket[BasketId] ==> BasketItems ==> OrderItems
        var basket = await basketRepository.GetBasketAsync(request.BasketId) ??
                     throw new BasketNotFoundException(request.BasketId);
        var orderItems = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var product = await unitOfWork.GetRepo<Product, int>().GetByIdAsync(item.Id)
                          ?? throw new ProductNotFoundException(item.Id);
            orderItems.Add(CreateOrderItem(item, product));
        }

        // DeliveryMethod
        var deliveryMethod = await unitOfWork.GetRepo<DeliveryMethod, int>().GetByIdAsync(request.DeliveryMethodId)
                             ?? throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

        // subtotal
        var orderRepo = unitOfWork.GetRepo<Order, Guid>();
        decimal subtotal = orderItems.Sum(item => item.Price * item.Quantity);
        var existingOrder = orderRepo.GetByIdAsync(new OrderPaymentSpecifications(basket.PaymentIntentId!));
        if (existingOrder != null)
        {
            orderRepo.Delete((await existingOrder)!);
        }
        // Create Order
        var order = new Order(email, address, deliveryMethod, subtotal, orderItems,basket.PaymentIntentId!);
        // Add and save db
        await orderRepo.AddAsync(order);
        await unitOfWork.SaveChangesAsync();
        // Map, return 
        return mapper.Map<OrderResultDto>(order);
    }

    private OrderItem CreateOrderItem(BasketItem item, Product product)
        => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity,
            product.Price);

    public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync()
    {
        var methods = await unitOfWork.GetRepo<DeliveryMethod, int>().GetAllAsync();
        return mapper.Map<IEnumerable<DeliveryMethodDto>>(methods);
    }
}