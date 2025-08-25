namespace Domain.Exceptions;

public class DeliveryMethodForBasketNotFoundException(string id) : NotFoundException($"The Delivery Method For Basket Id {id} Not Found")
{
    
}