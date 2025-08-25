using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.CustomerBasketDtos;

public class BasketItemDto
{
    public int Id { get; init; }
    public string ProductName { get; init; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }
    [Range(1, 99)]
    public int Quantity { get; init; }
}