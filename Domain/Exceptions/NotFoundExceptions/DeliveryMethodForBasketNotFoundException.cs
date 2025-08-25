namespace Domain.Exceptions;

public class DeliveryNotFoundException(string id) : NotFoundException($"The Delivery Method For Basket Id {id} Not Found")
{
    
}