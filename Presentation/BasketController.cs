using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;

namespace Presintation;

[Authorize] // should Login First
public class BasketController(IServiceManager serviceManager) : ApiController 
{
    // GET
    [HttpGet]
    public async Task<ActionResult<BasketDto>> Get(string id)
    {
        var basket=await serviceManager.BasketService.GetBasketAsync(id);
        if (basket == null) return NotFound();
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<BasketDto>> UpdateOrCreateBasket(BasketDto basketDto)
    {
        var basket = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
        if (basket == null) return NotFound();
        return Ok(basket);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        bool isDeleted = await serviceManager.BasketService.DeleteBasketAsync(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
}