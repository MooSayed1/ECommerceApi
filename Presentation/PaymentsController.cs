using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Interfaces;
using Shared.Dtos.CustomerBasketDtos;

namespace Presintation;

public class PaymentsController(IServiceManager serviceManager) : ApiController
{
    [HttpPost("{basketId}")]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var result = await serviceManager.PaymentService.CreatOrUpdateBasketIntentAsync(basketId);
        return Ok(result);
    }
    
}