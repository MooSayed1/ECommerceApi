using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.OrderDtos;

namespace Presintation;

public class OrderController(IServiceManager serviceManager) : ApiController
{
    [HttpPost]
    public async Task<ActionResult<OrderResultDto>> Create(OrderRequest orderRequest)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var orderDto = await serviceManager.OrderService.AddOrderAsync(orderRequest,email);
        return Ok(orderDto);
    }

    [HttpGet]
    public async ActionResult<IEnumerable<OrderResultDto>> GetAllOrder()
    {
        
    }
}