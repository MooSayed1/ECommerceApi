namespace Domain.Exceptions;

public sealed class BasketNotFoundException(string id) : NotFoundException($"Basket With id {id} Not Found")
{
}