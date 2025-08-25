using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.OrderDtos;

namespace Presintation;

[Authorize]
public class OrdersController(IServiceManager serviceManager) : ApiController
{
    [HttpPost]
    public async Task<ActionResult<OrderResultDto>> Create(OrderRequest orderRequest)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var orderDto = await serviceManager.OrderService.AddOrderAsync(orderRequest,email!);
        return Ok(orderDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResultDto>>> GetAllOrder()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var orders = await serviceManager.OrderService.GetAllOrdersByEmailAsync(email);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResultDto>> GetOrder(Guid id)
    {
        var order = await serviceManager.OrderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    [HttpGet("DeliveryMethods")]
    public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
    {
        return Ok(await serviceManager.OrderService.GetAllDeliveryMethodsAsync());
    }
    
}