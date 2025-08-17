using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.CustomerBasketDtos;

public class BasketItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    [Range(1, 99)]
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
}