using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Shared.Dtos.OrderDtos;

namespace Services.Abstraction.Interfaces;

public interface IOrderService
{
    // get order by id
    Task<OrderResultDto> GetOrderByIdAsync(Guid id); 
    // Get all orders for user by email
    Task<IEnumerable<OrderResultDto>> GetAllOrdersByEmailAsync(string? email);
    // Create order
    Task <OrderResultDto> AddOrderAsync(OrderRequest order, string email);
    // public bool CreateOrder(OrderParams orderParams);
    // get all delievery methods
    Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync();
}