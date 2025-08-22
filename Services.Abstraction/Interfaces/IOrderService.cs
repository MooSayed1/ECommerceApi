using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Shared.Dtos.OrderDtos;

namespace Services.Abstraction.Interfaces;

public interface IOrderService
{
    // get order by id
    public Task<OrderResultDto> GetOrderById(Guid id); 
    // Get all orders for user by email
    public Task<IEnumerable<OrderResultDto>> GetOrders(string? email);
    // Create order
    // public bool CreateOrder(OrderParams orderParams);
    // get all delievery methods
}