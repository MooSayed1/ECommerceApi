namespace Domain.Exceptions;

public class ProductNotFoundException(int id) : NotFoundException($"Product with id {id} is not found");