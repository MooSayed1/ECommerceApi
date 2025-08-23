namespace Domain.Exceptions;

public sealed class DeliveryMethodNotFoundException(int id) : NotFoundException($"The delivery method with id {id} not found")
{
    
}