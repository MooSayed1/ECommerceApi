namespace Domain.Exceptions;

public class ProductNotFoundExceptions(int id) : NotFoundExceptions($"Product with id {id} is not found");