namespace Domain.Exceptions;

public class OrderNotFoundException (Guid id): NotFoundException($"Order with id {id} not found")
{
    
}