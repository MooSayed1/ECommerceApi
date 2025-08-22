namespace Domain.Exceptions;

public sealed class BasketNotFoundException(int id) : NotFoundException($"Basket With id {id} Not Found")
{
}