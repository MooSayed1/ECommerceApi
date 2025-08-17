using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;

namespace Presintation;

[ApiController]
[Route("api/[controller]")] // localhost/api/basket
public class BasketController(IServiceManager serviceManager) : ControllerBase
{
    // GET
    [HttpGet("id")]
    public async Task<ActionResult<BasketDto>> Get(string id)
    {
        var basket=await serviceManager.BasketService.GetBasketAsync(id);
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<BasketDto>> UpdateOrCreateBasket(BasketDto basketDto)
    {
        var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
        return Ok(basket);
    }

    [HttpDelete("id")]
    public async Task<ActionResult> Delete(string id)
    {
        await serviceManager.BasketService.DeleteBasketAsync(id);
        return NoContent();
    }
}